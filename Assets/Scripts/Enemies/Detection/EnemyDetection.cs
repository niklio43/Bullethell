using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.Enemies.Detection
{
    public class EnemyDetection
    {
        public DetectionData Data;
        Enemy _owner;

        public EnemyDetection(Enemy owner)
        {
            _owner = owner;
            Data = new DetectionData();
        }

        public void Detect()
        {
            Data.Clear();
            Data.Add("Players", DetectEntities("Player", _owner.Stats.DetectionRadius));
            Data.Add("Enemies", DetectEntities("Enemy", _owner.Stats.DetectionRadius));
            Data.Add("Obstacles", DetectEntities("Obstacle", _owner.Stats.ObstacleDetectionRadius));

            if(_owner.Stats.MovementType == EnemyStats.EnemyMovementType.Grounded) {
                Data.Add("Obstacles", DetectEntities("GroundObstacle", _owner.Stats.ObstacleDetectionRadius));
            }
        }

        EntityData[] DetectEntities(string tag, float radius = 1)
        {
            List<EntityData> entities = new List<EntityData>();

            LayerMask mask = 1 << LayerMask.NameToLayer("Entity");
            Collider2D[] colliders = Physics2D.OverlapCircleAll(_owner.transform.position, radius, mask);

            foreach (var entity in colliders) {
                if(_owner.gameObject == entity.gameObject) { continue; }

                if(entity.tag == tag) {
                    entities.Add(new EntityData(entity));
                }
            }

            return entities.ToArray();
        }
    }
}