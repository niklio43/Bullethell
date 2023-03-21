using BulletHell.Enemies.Detection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.Enemies.Steering
{
    [CreateAssetMenu(fileName = "ObstacleAvoidanceBehaviour", menuName = "Enemies/Steering/Obstacle Avoidance Behaviour", order = 1)]
    public class ObstacleAvoidanceBehaviour : SteeringBehaviour
    {
        public override void GetSteering(AgentSteering steering, EnemyMovement movement)
        {
            Transform transform = movement.transform;
            Collider2D collider = movement.Collider;
            Enemy enemy = movement.Enemy;

            if (enemy.DetectionData.Count("Obstacles") == 0) { return; }

            foreach (EntityData obstacle in enemy.DetectionData["Obstacles"]) {
                Vector2 direction = obstacle.Collider.ClosestPoint(transform.position) - (Vector2)transform.position;
                float distance = direction.magnitude;

                float weight = collider.IsTouching(obstacle.Collider) ? 1 : Mathf.Clamp01(steering.AvoidanceRadius - distance) / steering.AvoidanceRadius;
                Vector2 directionNormalized = direction.normalized;

                for (int i = 0; i < steering.Danger.Count; i++) {
                    float result = Vector2.Dot(directionNormalized, steering.Directions[i]);
                    float value = result * weight;

                    if (value > steering.Danger[i])
                        steering.Danger[i] = value;
                }
            }
        }
    }
}
