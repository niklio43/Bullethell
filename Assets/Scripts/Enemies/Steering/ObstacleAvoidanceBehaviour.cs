using BulletHell.Enemies.Detection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.Enemies.Steering
{
    [CreateAssetMenu(fileName = "ObstacleAvoidanceBehaviour", menuName = "Enemies/Steering/ObstacleAvoidanceBehaviour", order = 1)]
    public class ObstacleAvoidanceBehaviour : SteeringBehaviour
    {
        [SerializeField] float _avoidanceRadius;

        public override void GetSteering(ContextMap danger, ContextMap interest, AgentSteering steering, DetectionData detectionData)
        {
            Transform transform = steering.transform;
            if(detectionData.ObstacleCount == 0) { return; }

            foreach (Collider2D obstacle in detectionData.Obstacles) {
                Vector2 direction = obstacle.ClosestPoint(transform.position) - (Vector2)transform.position;
                float distance = direction.magnitude;

                float weight = distance <= steering.ColliderRadius ? 1 : Mathf.Clamp01(_avoidanceRadius - distance) / _avoidanceRadius;
                Vector2 directionNormalized = direction.normalized;

                for (int i = 0; i < danger.Count; i++) {
                    float result = Vector2.Dot(directionNormalized, steering.Directions[i]);
                    float value = result * weight;

                    if (value > danger[i])
                        danger[i] = value;
                }
            }
        }
    }
}
