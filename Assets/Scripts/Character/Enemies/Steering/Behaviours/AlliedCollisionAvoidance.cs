using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Enemies.Detection;

namespace BulletHell.Enemies.Steering
{
    [CreateAssetMenu(fileName = "AlliedCollisionAvoidance", menuName = "Enemies/Steering/Allied Collision Avoidance", order = 1)]
    public class AlliedCollisionAvoidance : SteeringBehaviour
    {
        public override void GetSteering(AgentSteering steering, Enemy enemy)
        {
            Transform transform = enemy.transform;
            Collider2D collider = enemy.GetComponent<Collider2D>();

            if (enemy.DetectionData.Count("Enemies") == 0) { return; }

            foreach(EntityData obstacle in enemy.DetectionData["Enemies"]) {
                if (obstacle.gameObject == enemy.gameObject) { continue; }

                Vector2 direction = obstacle.Collider.ClosestPoint(transform.position) - (Vector2)transform.position;
                float distance = direction.magnitude;

                float weight = collider.IsTouching(obstacle.Collider) ? 1 : Mathf.Clamp01(steering.AvoidanceRadius - distance) / steering.AvoidanceRadius;
                weight = Mathf.Clamp01(weight);
                Vector2 oppositeDirectionNormalized = direction.normalized * -1;

                for (int i = 0; i < steering.Interest.Count; i++) {
                    float result = Vector2.Dot(oppositeDirectionNormalized, steering.Directions[i]);
                    float value = result * weight * .5f;

                    if(value > steering.Interest[i])
                        steering.Interest[i] = value;
                }
            }
        }
    }
}
