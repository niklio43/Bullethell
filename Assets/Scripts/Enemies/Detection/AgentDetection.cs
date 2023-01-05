using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.Enemies.Detection
{
    public class AgentDetection : MonoBehaviour
    {
        public bool ShowGizmo = true;

        [Range(0, 10)] public float Radius = 2;
        [Range(0, 10)] public float ObstacleDetectionRadius = 1;

        public DetectionData Data = new DetectionData();

        private void Start()
        {
            InvokeRepeating(nameof(Detect), 0, 0.1f);
        }

        public void Detect()
        {
            Data.Clear();
            Data.Add("Player", DetectEntities("Player", Radius));
            Data.Add("Enemy", DetectEntities("Enemy", Radius));
            Data.Add("Obstacle", DetectEntities("Obstacle", ObstacleDetectionRadius));
        }

        private EntityData[] DetectEntities(string tag = "", float radius = 1)
        {
            List<EntityData> entities = new List<EntityData>();

            LayerMask mask = 1 << LayerMask.NameToLayer("Entity");
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, mask);

            foreach (var entity in colliders) {
                if(gameObject == entity.gameObject) { continue; }

                if(tag == "" || entity.tag == tag) {
                    entities.Add(new EntityData(entity));
                }
            }

            return entities.ToArray();
        }
    }
}