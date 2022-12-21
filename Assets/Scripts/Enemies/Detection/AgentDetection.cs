using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.Enemies.Detection
{
    public class AgentDetection : MonoBehaviour
    {
        public bool ShowGizmo = true;

        [Header("FOV")]
        [Range(0, 360)] public float Angle = 45;
        public float Radius = 2;
        public float innerRadius = 1;
        public float ObstacleDetectionRadius = 1;

        [Header("Runtime Data")]
        public DetectionData Data = new DetectionData();

        private void Start()
        {
            InvokeRepeating(nameof(Detect), 0, 0.1f);
        }

        public void Detect()
        {
            Data.Obstacles = DetectObstacles();
            Data.Players = DetectPlayers();
            Data.Friendlies = DetectFriendlies();
        }

        public Collider2D[] DetectPlayers()
        {
            LayerMask mask = 1 << LayerMask.NameToLayer("Player");
            LayerMask obstacleMask = 1 << LayerMask.NameToLayer("Obstacle");

            return DetectInFOV(mask, obstacleMask);
        }

        public Collider2D[] DetectFriendlies()
        {
            LayerMask mask = 1 << LayerMask.NameToLayer("Enemy");
            LayerMask obstacleMask = 1 << LayerMask.NameToLayer("Obstacle");

            return DetectInFOV(mask, obstacleMask);
        }

        public Collider2D[] DetectObstacles()
        {
            LayerMask mask = 1 << LayerMask.NameToLayer("Obstacle");
            Collider2D[] obstacles = Physics2D.OverlapCircleAll(transform.position, ObstacleDetectionRadius, mask);

            return obstacles;
        }

        public Collider2D[] DetectInFOV(LayerMask mask, LayerMask obstacleMask)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, Radius, mask);

            List<Collider2D> visable = new List<Collider2D>();

            foreach (Collider2D item in colliders) {

                if (TargetInView(item.transform, obstacleMask)) {
                    visable.Add(item);
                }
            }

            return visable.ToArray();
        }

        public bool TargetInView(Transform target, LayerMask obstacleMask)
        {
            float dist = Vector2.Distance(transform.position, target.position);
            if (dist > Radius) { return false; }

            Vector2 direction = (target.position - transform.position).normalized;

            if (dist < innerRadius || Vector2.Angle(transform.up, direction) < Angle / 2) {
                if (!Physics.Raycast(transform.position, direction, dist, obstacleMask)) {
                    return true;
                }
            }

            return false;
        }

    }
}