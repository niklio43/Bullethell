using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.Map.Generation
{
    public static class GenerationUtilities
    {
        public static int CellSize => 20;

        public static Vector2 GridToWorldPosition(Vector2Int pos) => GridToWorldPosition(pos.x, pos.y);
        public static Vector2 GridToWorldPosition(int x, int y)
        {
            Vector2Int center = new Vector2Int(CellSize / 2, CellSize / 2);

            int posX = x * CellSize;
            int posY = y * CellSize;

            return new Vector2Int(posX, posY) + center;
        }

        public static Vector2 WorldToGridPosition(Vector2Int pos) => WorldToGridPosition(pos.x, pos.y);
        public static Vector2Int WorldToGridPosition(float x, float y)
        {
            int i_x = Mathf.RoundToInt(x);
            int i_y = Mathf.RoundToInt(y);

            Vector2Int pos = new Vector2Int(i_x / CellSize, i_y / CellSize);

            return pos;
        }



    }
}
