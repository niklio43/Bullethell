using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace BulletHell.Map.Generation
{
    using Random = UnityEngine.Random;

    public class Generator
    {
        #region Private Fields
        private readonly GenerationConfig _config;
        private GenerationGrid _grid;
        #endregion

        #region Public Methods
        public Generator(GenerationConfig config)
        {
            _config = config;
            _grid = new GenerationGrid(_config.SizeX, _config.SizeY);
        }
        public void BeginGeneration(Action<List<Room>> OnCompletedGeneration)
        {
            if (!Application.isPlaying) { throw new Exception("Cannot generate outside of play-mode"); }
            OnCompletedGeneration.Invoke(Generate());
        }

        #endregion

        #region Private Methods
        List<Room> Generate()
        {
            GenerateLayout();
            PopulateLayout(out List<Room> rooms);
            ConnectDoors();

            return rooms;
        }

        #region Generate Layout
        void GenerateLayout()
        {
            Vector2Int startPosition = new Vector2Int(_config.SizeX / 2, _config.SizeY / 2);

            RandomWalk(startPosition);
            DLA();
        }

        void RandomWalk(Vector2Int startPosition)
        {
            int amountOfRooms = 0;

            if (_grid[startPosition.x, startPosition.y] == null) {
                _grid[startPosition.x, startPosition.y] = new GenerationCell(_grid, startPosition);
                amountOfRooms++;
            }

            while (amountOfRooms < _config.RandomWalkSteps) {

                Vector2Int pos = startPosition;

                while (amountOfRooms < _config.RandomWalkSteps) {
                    int dir = Random.Range(0, 4);
                    Direction direction = (Direction)dir;

                    pos += direction.GetVector();

                    if (pos.x < 0 || pos.x >= _config.SizeX ||
                        pos.y < 0 || pos.y >= _config.SizeY) { break; }

                    if (_grid[pos.x, pos.y] == null) {
                        _grid[pos.x, pos.y] = new GenerationCell(_grid, pos);
                        amountOfRooms++;
                    }
                }
            }

           
        }
        void DLA()
        {
            int amountOfRooms = _config.RandomWalkSteps;

            while (amountOfRooms < _config.Size) {
                int x = Random.Range(0, _config.SizeX);
                int y = Random.Range(0, _config.SizeY);

                if (_grid[x, y] != null) { continue; }

                Vector2Int pos = new Vector2Int(x, y);

                if (_grid.GetNeighbouringCardinalCells(pos).Count > 0) {
                    _grid[x, y] = new GenerationCell(_grid, pos);
                    amountOfRooms++;
                }
            }
        }

        #endregion

        #region Populate Layout

        private void PopulateLayout(out List<Room> rooms)
        {
            rooms = new List<Room>();
            PlaceStartEndRoom(rooms);
            PopulateCells(rooms, _config.BigRooms, _config.MaxBigRooms);
            PopulateCells(rooms, _config.SmallRooms);
        }

        private void PlaceStartEndRoom(List<Room> rooms)
        {
            Vector2Int Center = _grid.GetCenterPosition();

            GenerationCell check = null; 
            float d = 0;

            foreach (GenerationCell cell in _grid.AliveCells) {
                if (cell.IsOccupied()) continue;
                float dist = Vector2.Distance(Center, cell.GetGridPosition());
                if(dist > d) {
                    check = cell;
                    d = dist;
                }
            }

            rooms.Add(PlaceRoom(_config.StartRoom, check.GetGridPosition()));

            GenerationCell check_2 = null;
            d = 0;

            foreach (GenerationCell cell in _grid.AliveCells) {
                if (cell.IsOccupied()) continue;
                float dist = Vector2.Distance(check.GetGridPosition(), cell.GetGridPosition());
                if (dist > d) {
                    check_2 = cell;
                    d = dist;
                }
            }

            rooms.Add(PlaceRoom(_config.StartRoom, check_2.GetGridPosition()));
        }

        private void PopulateCells(List<Room> rooms, Room[] roomOriginals, int maxAmount = 0)
        {
            if (maxAmount == 0) maxAmount = _config.Size;
            int amountPlaced = 0;


            foreach (GenerationCell cell in _grid.AliveCells) {
                if (amountPlaced >= maxAmount) { break; }

                System.Random r = new System.Random();
                Room[] shuffledArray = roomOriginals.OrderBy(e => r.NextDouble()).ToArray();

                foreach (Room room in shuffledArray) {
                    if (!ValidRoomPlacement(room, cell.GetGridPosition())) { continue; }
                    rooms.Add(PlaceRoom(room, cell.GetGridPosition()));
                    amountPlaced++;
                }
            }
        }

        #endregion

        #region Connect Rooms
        private void ConnectDoors()
        {
            foreach (GenerationCell cell in _grid.AliveCells) {
                foreach (Door door in cell.GetDoors()) {
                    if (door.IsConnected()) { continue; }
                    Vector2Int gridPos = cell.GetGridPosition();
                    Vector2Int check = gridPos + door.GetOrientation().GetVector();

                    GenerationCell gridCell = _grid.GetCell(check);

                    if (gridCell == null) {
                        door.CloseDoor();
                        continue;
                    }

                    Direction requiredOrientation = door.GetConnecteeOrientation();

                    foreach (Door conecteeDoor in gridCell.GetDoors()) {
                        if (conecteeDoor.GetOrientation() == requiredOrientation) {
                            door.OpenDoor(conecteeDoor);
                            conecteeDoor.OpenDoor(door);
                        }
                    }
                }
            }
        }

        #endregion

        #region Room Placement
        private bool ValidRoomPlacement(Room roomOriginal, Vector2Int pos) => ValidRoomPlacement(roomOriginal, pos.x, pos.y);
        private bool ValidRoomPlacement(Room roomOriginal, int x, int y)
        {
            foreach (RoomCell cell in roomOriginal.Cells) {
                Vector2Int check = new Vector2Int(x + cell.x, y + cell.y);

                GenerationCell gridCell = _grid.GetCell(check);
                if (gridCell == null || gridCell.IsOccupied()) return false;
            }

            return true;
        }

        private Room PlaceRoom(Room roomOriginal, Vector2Int pos) => PlaceRoom(roomOriginal, pos.x, pos.y);
        private Room PlaceRoom(Room roomOriginal, int x, int y)
        {
            if(!_grid.IsWithinBounds(x, y)) { throw new Exception("Attempted to place rooms outside of bounds!"); }
            Room room = GameObject.Instantiate(roomOriginal, GenerationUtilities.GridToWorldPosition(x, y), Quaternion.identity);
            room.name = roomOriginal.name;
            room.OnCreation(new Vector2Int(x, y));

            foreach (RoomCell cell in room.Cells) {
                _grid[x + cell.x, y + cell.y].SetOccupant(room);
                _grid[x + cell.x, y + cell.y].SetDoors(cell.Doors);
            }

            return room;
        }

        #endregion

        #endregion

        #region Gizmos
        //private void OnDrawGizmos()
        //{
        //    if (_grid != null)
        //        _grid.OnDrawGizmos();

        //    DrawGrid();
        //}

        //void DrawGrid()
        //{
        //    if (_config == null || !_showGrid) { return; }
        //    Gizmos.color = Color.gray;

        //    for (int i = 0; i <= _config.SizeY; i++) {
        //        Vector3 step = Vector3.up * i * GenerationUtilities.CellSize;
        //        Vector3 width = new Vector3(GenerationUtilities.CellSize * _config.SizeX, 0, 0);

        //        Gizmos.DrawLine(transform.position + step, transform.position + step + width);
        //    }
        //    for (int i = 0; i <= _config.SizeX; i++) {
        //        Vector3 step = Vector3.right * i * GenerationUtilities.CellSize;
        //        Vector3 height = new Vector3(0, GenerationUtilities.CellSize * _config.SizeY, 0);

        //        Gizmos.DrawLine(transform.position + step, transform.position + step + height);
        //    }
        //}
        #endregion
    }
}
