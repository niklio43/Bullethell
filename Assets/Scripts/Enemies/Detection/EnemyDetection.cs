using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.Enemies.Detection
{
    public class EnemyDetection : MonoBehaviour
    {
        public bool DrawGizmos = false;
        [Range(0, 10)] public float DetectionRadius = 2;
        [Range(0, 10)] public float ObstacleDetectionRadius = 1;

        Enemy _enemy;

        private void Awake()
        {
            _enemy = GetComponent<Enemy>();
        }

        private void Start()
        {
            InvokeRepeating(nameof(Detect), 0, .05f);
        }

        public DetectionData Detect()
        {
            DetectionData data = new DetectionData();

            data.Add("Players", DetectEntities("Player", DetectionRadius));
            data.Add("Enemies", DetectEntities("Enemy", DetectionRadius));
            data.Add("Obstacles", DetectEntities("Obstacle", ObstacleDetectionRadius, 1 << LayerMask.NameToLayer("Obstacle")));
            
            if(_enemy.MovementType == Enemy.EnemyMovmentType.Grounded) {
                data.Add("Obstacles", DetectEntities("GroundObstacle", ObstacleDetectionRadius));
            }

            return data;
        }

        EntityData[] DetectEntities(string tag, float radius)
        {
            LayerMask defaultMask = 1 << LayerMask.NameToLayer("Entity");
            return DetectEntities(tag, radius, defaultMask);
        }

        EntityData[] DetectEntities(string tag, float radius, LayerMask mask)
        {
            List<EntityData> entities = new List<EntityData>();

            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, mask);

            foreach (var entity in colliders) {
                if(gameObject == entity.gameObject) { continue; }

                if(entity.tag == tag) {
                    entities.Add(new EntityData(entity));
                }
            }

            return entities.ToArray();
        }
    }
}