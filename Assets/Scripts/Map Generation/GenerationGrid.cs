using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.Map.Generation
{
    public class GenerationGrid
    {
        #region Public Fields
        public GenerationCell this[int x, int y] { get { return _grid[x, y]; } set { _grid[x, y] = value; } }
        public Vector2Int GetSize => new Vector2Int(_sizeX, _sizeY);
        public List<GenerationCell> AliveCells;
        #endregion

        #region Private Fields
        GenerationCell[,] _grid;
        readonly int _sizeX, _sizeY;
        #endregion

        #region Public Methods
        public GenerationGrid(int sizeX, int sizeY)
        {
            _sizeX = sizeX;
            _sizeY = sizeY;

            AliveCells = new List<GenerationCell>();
            _grid = new GenerationCell[_sizeX, _sizeY];
        }
        public bool IsWithinBounds(Vector2Int pos) => IsWithinBounds(pos.x, pos.y);
        public bool IsWithinBounds(float x, float y)
        {
            return (x > 0 && x < _sizeX && y > 0 && y < _sizeY);
        }

        public Vector2Int GetCenterPosition()
        {
            Vector2Int a = new Vector2Int(0, 0);

            foreach (GenerationCell cell in AliveCells) {
                a += cell.GetGridPosition();
            }

            a /= AliveCells.Count;
            return a;
        }


        public List<GenerationCell> GetNeighbouringCardinalCells(Vector2Int pos) => GetNeighbouringCardinalCells(pos.x, pos.y);
        public List<GenerationCell> GetNeighbouringCardinalCells(int x, int y)
        {
            List<GenerationCell> neighbours = new List<GenerationCell>();
            Vector2Int pos = new Vector2Int(x, y);

            for (int i = 0; i < 4; i++) {
                Direction direction = (Direction)i;
                Vector2Int check = pos + direction.GetVector();

                if (check.x < 0 || check.x >= _sizeX || check.y < 0 || check.y >= _sizeY) { continue; }

                if (_grid[check.x, check.y] != null) {
                    neighbours.Add(_grid[check.x, check.y]);
                }
            }

            return neighbours;
        }

        public GenerationCell GetCell(Vector2Int pos) => GetCell(pos.x, pos.y);
        public GenerationCell GetCell(int x, int y)
        {
            if (x < 0 || x >= _sizeX || y < 0 || y >= _sizeY) return null;
            return _grid[x, y];
        }
        #endregion

        #region Gizmos
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
        #endregion
    }

    public class GenerationCell
    {

        #region Private Fields
        readonly int _posX, _posY;
        readonly GenerationGrid _grid;

        List<GenerationCell> Neighbours;

        Room _occupant = null;
        Door[] _doors;
        #endregion

        #region Getters & Setters
        public bool IsOccupied() => _occupant != null;
        public void SetOccupant(Room room) => _occupant = room;
        public Room GetOccupant() => _occupant;
        //
        public Door[] GetDoors() => _doors;
        public void SetDoors(Door[] doors) => _doors = doors;
        #endregion

        public GenerationCell(GenerationGrid grid, int x, int y)
        {
            _grid = grid;
            _posX = x;
            _posY = y;
            grid.AliveCells.Add(this);
            GetAndUpdateNeighbours();
        }
        public GenerationCell(GenerationGrid grid, Vector2Int pos)
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
            foreach (GenerationCell neighbour in Neighbours) {
                neighbour.AddNeighbour(this);
            }
        }

        public void AddNeighbour(GenerationCell neighbour)
        {
            if (Neighbours.Contains(neighbour)) { return; }
            Neighbours.Add(neighbour);
        }

        public Vector2Int GetGridPosition() => new Vector2Int(_posX, _posY);
        public Vector2 GetWorldPositon() => GenerationUtilities.GridToWorldPosition(_posX, _posY);

        #region Gizmos
        public void Draw()
        {
            Gizmos.color = Color.green;
            if (_occupant != null) {
                Gizmos.color = _occupant.ColorCoding;
            }
            Gizmos.DrawCube(GetWorldPositon(), Vector2.one * GenerationUtilities.CellSize);
        }
        #endregion
    }
}
