using BulletHell.Map.Generation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BulletHell.Map
{
    public class Room : MonoBehaviour
    {
        public Door[] Doors;
        public Vector2Int[] Cells;
        public Color colorCoding;

        const int _cellSize = 20;
        private void Awake()
        {
            
        }

        public bool ValidPlacement(MapGrid grid, Vector2Int pos) => ValidPlacement(grid, pos.x, pos.y);
        public bool ValidPlacement(MapGrid grid, int x, int y)
        {
            foreach (Vector2Int cell in Cells) {
                if (grid[x + cell.x, y + cell.y] == null) return false;
                if (grid[x + cell.x, y + cell.y].IsOccupied()) { return false; }
            }

            return true;
        }

        public void Place(MapGrid grid, Vector2Int pos) => Place(grid, pos.x, pos.y);
        public void Place(MapGrid grid, int x, int y)
        {
            foreach (Vector2Int cell in Cells) {
                grid[x + cell.x, y + cell.y].SetRoom(this);
                if(cell != Vector2Int.zero)
                grid[x + cell.x, y + cell.y].Composite = true;
            }
        }

        public bool IsOverLapping()
        {
            return false;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;

            for (int i = 0; i < Cells.Length; i++) {
                Vector2 pos = (Vector2)transform.position + (Cells[i] * _cellSize);

                Gizmos.DrawWireCube(pos, Vector2.one * _cellSize);
            }
        }
    }
}
