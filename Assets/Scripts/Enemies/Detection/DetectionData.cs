using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BulletHell.Enemies.Detection
{
    [System.Serializable]
    public class DetectionData
    {
        public Collider2D[] Obstacles;
        public Collider2D[] Players;
        public Collider2D[] Friendlies;

        public int ObstacleCount => (Obstacles == null) ? 0 : Obstacles.Length;
        public int PlayersCount => (Players == null) ? 0 : Players.Length;
        public int FriendliesCount => (Friendlies == null) ? 0 : Friendlies.Length;
    }
}
