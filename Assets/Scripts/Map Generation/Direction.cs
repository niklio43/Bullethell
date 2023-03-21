using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BulletHell.Map
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    public static class DirectionUtils
    {
        public static Vector2Int GetVector(this Direction direction)
        {
            switch (direction) {
                case Direction.Up:
                    return Vector2Int.up;
                case Direction.Down:
                    return Vector2Int.down;
                case Direction.Left:
                    return Vector2Int.left;
                case Direction.Right:
                    return Vector2Int.right;
            }

            return Vector2Int.zero;
        }
    }



}
