using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BulletHell.Map.Generation
{
    using Random = UnityEngine.Random;

    public class Generator : MonoBehaviour
    {
        [SerializeField] GenerationConfig _config;
        [SerializeField] bool _showGrid = true;
        
        MapGrid _grid;


        [ContextMenu("Generate DLA")]
        public void GenerateWithDLA()
        {
            if(!Application.isPlaying) { throw new Exception("Cannot generate outside of play-mode"); }

            foreach (Transform child in transform) {
                Destroy(child.gameObject);
            }

            _grid = new MapGrid(_config.SizeX, _config.SizeY, _config.CellSize);
            Random.InitState(_config.GetSeed());
            StartCoroutine(Generate());
        }

        IEnumerator Generate()
        {
            Vector2Int startPosition = new Vector2Int(_config.SizeX / 2, _config.SizeY / 2);

            yield return RandomWalk(startPosition);
            yield return DLA(startPosition);
            yield return PopulateCells(_config.BigRooms, _config.MaxBigRooms);
            yield return PopulateCells(_config.SmallRooms);
            yield return PlacePrefabs();
            yield return ConnectDoors();
        }

        IEnumerator DLA(Vector2Int startPosition)
        {
            int amountOfRooms = _config.RandomWalkSteps;

            while(amountOfRooms < _config.Size) {
                int x = Random.Range(0, _config.SizeX);
                int y = Random.Range(0, _config.SizeY);

                if(_grid[x, y] != null) { continue; }

                Vector2Int pos = new Vector2Int(x, y);

                if(_grid.GetNeighbouringCardinalCells(pos).Count > 0) {
                    _grid[x, y] = new MapCell(_grid, pos);
                    amountOfRooms++;
                }
            }

            yield return null;
        }
        IEnumerator RandomWalk(Vector2Int startPosition)
        {
            int amountOfRooms = 0;

            if (_grid[startPosition.x, startPosition.y] == null) {
                _grid[startPosition.x, startPosition.y] = new MapCell(_grid, startPosition);
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
                        _grid[pos.x, pos.y] = new MapCell(_grid, pos);
                        amountOfRooms++;
                    }
                }
            }

            yield return null;
        }
        IEnumerator PopulateCells(Room[] rooms, int maxAmount = 0)
        {
            if (maxAmount == 0) maxAmount = _config.Size;
            int amountPlaced = 0;


            foreach (MapCell cell in _grid.AliveCells) {
                if(amountPlaced >= maxAmount) { break; }

                System.Random r = new System.Random();
                Room[] shuffledArray = rooms.OrderBy(e => r.NextDouble()).ToArray();

                foreach (Room room in shuffledArray) {
                    if (!room.ValidPlacement(_grid, cell.GetGridPosition())) { continue; }
                    room.Place(_grid, cell.GetGridPosition());
                    amountPlaced++;
                }
            }

            yield return null;
        }
        IEnumerator PlacePrefabs()
        {
            foreach (MapCell cell in _grid.AliveCells) {
                if(cell.Composite) { continue; }
                Instantiate(cell.GetRoom(), cell.GetWorldPositon(), Quaternion.identity, transform);
            }

            yield return null;
        }

        IEnumerator ConnectDoors()
        {
            foreach (MapCell cell in _grid.AliveCells) {
                Room room = cell.GetRoom();

            }

            yield return null;
        }

        private void OnDrawGizmos()
        {
            if (_grid != null)
                _grid.OnDrawGizmos();

            DrawGrid();
        }

        void DrawGrid()
        {
            if (_config == null || !_showGrid) { return; }
            Gizmos.color = Color.gray;

            for (int i = 0; i <= _config.SizeY; i++) {
                Vector3 step = Vector3.up * i * _config.CellSize;
                Vector3 width = new Vector3(_config.CellSize * _config.SizeX, 0, 0);

                Gizmos.DrawLine(transform.position + step, transform.position + step + width);
            }
            for (int i = 0; i <= _config.SizeX; i++) {
                Vector3 step = Vector3.right * i * _config.CellSize;
                Vector3 height = new Vector3(0, _config.CellSize * _config.SizeY, 0);

                Gizmos.DrawLine(transform.position + step, transform.position + step + height);
            }
        }

    }
}
