using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.Map.Generation
{
    public class MapGrid
    {
        public MapCell this[int x, int y] { get { return _grid[x, y]; } set { _grid[x, y] = value; } }

        MapCell[,] _grid;

        public List<MapCell> AliveCells;

        readonly int _sizeX, _sizeY;

        int _cellSize => GenerationUtilities.CellSize;
        public Vector2Int GetSize => new Vector2Int(_sizeX, _sizeY);

        public MapGrid(int sizeX, int sizeY)
        {
            _sizeX = sizeX;
            _sizeY = sizeY;

            AliveCells = new List<MapCell>();
            _grid = new MapCell[_sizeX, _sizeY];
        }

        public Vector2 GridToWorldPosition(Vector2Int pos) => GridToWorldPosition(pos.x, pos.y);
        public Vector2 GridToWorldPosition(int x, int y)
        {
            if (x < 0 || y < 0) { return Vector2Int.zero; }
            if (x > _sizeX || y > _sizeY) { return Vector2Int.zero; }

            Vector2Int center = new Vector2Int(_cellSize / 2, _cellSize / 2);

            int posX = x * _cellSize;
            int posY = y * _cellSize;

            return new Vector2Int(posX, posY) + center;
        }

        public Vector2 WorldToGridPosition(Vector2Int pos) => WorldToGridPosition(pos.x, pos.y);
        public Vector2Int WorldToGridPosition(float x, float y)
        {
            int i_x = Mathf.RoundToInt(x);
            int i_y = Mathf.RoundToInt(y);

            Vector2Int pos = new Vector2Int(i_x / _cellSize, i_y / _cellSize);

            return pos;
        }


        public Vector2Int GetCenterPosition()
        {
            Vector2Int a = new Vector2Int(0, 0);

            foreach (MapCell cell in AliveCells) {
                a += cell.GetGridPosition();
            }

            a /= AliveCells.Count;
            return a;
        }

        public List<MapCell> GetNeighbouringCardinalCells(Vector2Int pos) => GetNeighbouringCardinalCells(pos.x, pos.y);
        public List<MapCell> GetNeighbouringCardinalCells(int x, int y)
        {
            List<MapCell> neighbours = new List<MapCell>();
            Vector2Int pos = new Vector2Int(x, y);

            for (int i = 0; i < 4; i++) {
                Direction direction = (Direction)i;
                Vector2Int check = pos + direction.GetVector();
                
                if (check.x < 0 || check.x >= _sizeX || check.y < 0 || check.y >= _sizeY) { continue; }
                
                if(_grid[check.x, check.y] != null) {
                    neighbours.Add(_grid[check.x, check.y]);
                }
            }

            return neighbours;
        }

        public MapCell GetCell(Vector2Int pos) => GetCell(pos.x, pos.y);
        public MapCell GetCell(int x, int y)
        {
            if (x < 0 || x >= _sizeX || y < 0 || y >= _sizeY) return null;
            return _grid[x, y];
        }


        public void OnDrawGizmos()
        {
            if (_grid == null) { return; }
            Gizmos.color = Color.green;
            for (int x = 0; x < _sizeX; x++) {
                for (int y = 0; y < _sizeY; y++) {
                    if (_grid[x, y] != null)
                        _grid[x, y].Draw();
                }
            }
        }
    }

    public class MapCell
    {
        readonly int _posX, _posY;
        readonly MapGrid _grid;

        List<MapCell> Neighbours;

        Room _occupant = null;
        Door[] _doors;

        #region Getters & Setters
        public bool IsOccupied() => _occupant != null;
        public void SetOccupant(Room room) => _occupant = room;
        public Room GetOccupant() => _occupant; 
        //
        public Door[] GetDoors() => _doors;
        public void SetDoors(Door[] doors) => _doors = doors;
        #endregion

        public MapCell(MapGrid grid, int x, int y)
        {
            _grid = grid;
            _posX = x;
            _posY = y;
            grid.AliveCells.Add(this);
            GetAndUpdateNeighbours();
        }
        public MapCell(MapGrid grid, Vector2Int pos)
        {
            _grid = grid;
            _posX = pos.x;
            _posY = pos.y;
            grid.AliveCells.Add(this);
            GetAndUpdateNeighbours();
        }

        void GetAndUpdateNeighbours()
        {
            Neighbours = _grid.GetNeighbouringCardinalCells(_posX, _posY);
            foreach (MapCell neighbour in Neighbours) {
                neighbour.AddNeighbour(this);
            }
        }

        public void AddNeighbour(MapCell neighbour)
        {
            if(Neighbours.Contains(neighbour)) { return; }
            Neighbours.Add(neighbour);
        }

        public Vector2Int GetGridPosition() => new Vector2Int(_posX, _posY);
        public Vector2 GetWorldPositon() => _grid.GridToWorldPosition(_posX, _posY);

        #region Gizmos
        public void Draw()
        {
            Gizmos.color = Color.green;
            if(_occupant != null) {
                Gizmos.color = _occupant.colorCoding;
            }
            Gizmos.DrawCube(GetWorldPositon(), Vector2.one * GenerationUtilities.CellSize);
        }
        #endregion
    }
}
