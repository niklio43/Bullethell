using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BulletHell.Enemies.Detection
{
    public class DetectionData
    {
        public Collider2D[] Obstacles;
        public Collider2D[] Players;
        public Collider2D[] Friendlies;

        public int ObstacleCount => (Obstacles == null) ? 0 : Obstacles.Length;
        public int PlayersCount => (Obstacles == null) ? 0 : Obstacles.Length;
        public int FriendliesCount => (Obstacles == null) ? 0 : Obstacles.Length;
    }
}
