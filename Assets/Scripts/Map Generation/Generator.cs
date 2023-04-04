using System;
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
        public void BeginGeneration(Action<GenerationData> OnCompletedGeneration)
        {
            if (!Application.isPlaying) { throw new Exception("Cannot generate outside of play-mode"); }
            OnCompletedGeneration.Invoke(Generate());
        }

        #endregion

        #region Private Methods
        GenerationData Generate()
        {
            GenerateLayout();
            PopulateLayout(out List<Room> rooms);
            ConnectDoors();

            LayoutBounds bounds = CalculateBounds();

            return new GenerationData(rooms, bounds);
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

                    if (pos.x < 0 || pos.x > _config.SizeX ||
                        pos.y < 0 || pos.y > _config.SizeY) { break; }

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

                if (_grid.GetNeighbouringCardinalCells(pos).Count > 0 && _grid.IsWithinBounds(pos)) {
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
            PlaceStartEndRoom(rooms, _config.SpecialRooms);
            PopulateCells(rooms, _config.BigRooms);
            PopulateCells(rooms, _config.SmallRooms);
            PopulateCells(rooms, _config.DefaultRooms);
        }

        private void PlaceStartEndRoom(List<Room> rooms, SpecialRoomConfig[] roomOriginals)
        {
            Dictionary<SpecialRoomConfig, Room> pairs = new Dictionary<SpecialRoomConfig, Room>();
            Vector2 center = _grid.GetCenterPosition();
            List<GenerationCell> sortedList = _grid.AliveCells.OrderBy(c => Vector2.Distance(center, c.GetGridPosition())).Reverse().ToList();


            foreach (SpecialRoomConfig roomConfig in roomOriginals) {
                foreach (GenerationCell cell in sortedList) {
                    bool check = false;

                    foreach (var pair in pairs) {
                        Vector2Int p = cell.GetGridPosition();
                        Vector2Int c = pair.Value.GetCenterPositionAsInt();
                        int s = pair.Key.OccupyingSquare;

                        if (p.x > c.x - s && p.x < c.x + s &&
                            p.y > c.y - s && p.y < c.y + s)
                            check = true;
                    }

                    if (check) continue;

                    Room placedRoom = PlaceRoom(roomConfig.RoomOriginal, cell.GetGridPosition());

                    rooms.Add(placedRoom);
                    pairs.Add(roomConfig, placedRoom);
                    break;
                }
            }
        }


        private void PopulateCells(List<Room> rooms, RoomConfig[] roomOriginals)
        {
            if (roomOriginals.Length == 0) return;
            Dictionary<RoomConfig, int> roomAmountPairs = new Dictionary<RoomConfig, int>();

            foreach (RoomConfig roomConfig in roomOriginals) {
                roomAmountPairs.Add(roomConfig, 0);
            }

            foreach (GenerationCell cell in _grid.AliveCells) {
                System.Random r = new System.Random();
                RoomConfig[] shuffledArray = roomOriginals.OrderBy(e => r.NextDouble()).ToArray();

                bool check = true;

                foreach (RoomConfig roomConfig in shuffledArray) {
                    if (roomConfig.MaxAmount != -1 && roomConfig.MaxAmount <= roomAmountPairs[roomConfig]) {
                        continue;
                    }

                    check = false;
                    if (!ValidRoomPlacement(roomConfig.RoomOriginal, cell.GetGridPosition())) { continue; }
                    rooms.Add(PlaceRoom(roomConfig.RoomOriginal, cell.GetGridPosition()));
                    roomAmountPairs[roomConfig]++;
                }

                if (check) break;
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
            if (!_grid.IsWithinBounds(x, y)) { throw new Exception("Attempted to place rooms outside of bounds!"); }
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

        private LayoutBounds CalculateBounds()
        {
            int minX = _grid.AliveCells[0].GetGridPosition().x;
            int minY = _grid.AliveCells[0].GetGridPosition().y;
            int maxX = _grid.AliveCells[0].GetGridPosition().x;
            int maxY = _grid.AliveCells[0].GetGridPosition().y;

            for (int i = 1; i < _grid.AliveCells.Count; i++) {
                Vector2Int pos = _grid.AliveCells[i].GetGridPosition();
                if(pos.x > maxX) {
                    maxX = pos.x;
                }
                if(pos.y > maxY) {
                    maxY = pos.y;
                }
                if(pos.x < minX) {
                    minX = pos.x;
                }
                if(pos.y < minY) {
                    minY = pos.y;
                }
            }

            int width = (1 + maxX - minX);
            int height = (1 + maxY - minY);
            Vector2 center = new Vector2(minX, minY) + new Vector2(width / 2f, height / 2f);

            width *= GenerationUtilities.CellSize;
            height *= GenerationUtilities.CellSize;
            center *= GenerationUtilities.CellSize;

            return new LayoutBounds() {Width = width, Height = height, Center = center};
        }
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

    public class GenerationData{
        public List<Room> Rooms;
        public LayoutBounds Bounds;

        public GenerationData(List<Room> rooms, LayoutBounds bounds)
        {
            this.Rooms = rooms;
            this.Bounds = bounds;
        }
    }

    public struct LayoutBounds
    {
        public int Width, Height;
        public Vector2 Center;
    }

}
