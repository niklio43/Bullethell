using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Enemies.Detection;

namespace BulletHell.Enemies.Steering
{
    [CreateAssetMenu(fileName = "AlliedCollisionAvoidance", menuName = "Enemies/Steering/Allied Collision Avoidance", order = 1)]
    public class AlliedCollisionAvoidance : SteeringBehaviour
    {
        #region Public Methods
        public override void GetSteering(AgentSteering steering, EnemyMovement movement)
        {
            Transform transform = movement.transform;
            Collider2D collider = movement.Collider;
            Enemy enemy = movement.Enemy;

            if (enemy.DetectionData.Count("Enemies") == 0) { return; }

            foreach(EntityData obstacle in enemy.DetectionData["Enemies"]) {
                if (obstacle.gameObject == enemy.gameObject) { continue; }

                Vector2 direction = obstacle.transform.position - transform.position;
                float distance = direction.magnitude;

                float weight = collider.IsTouching(obstacle.Collider) ? 1 : Mathf.Clamp01(1.2f - distance / 1.2f);
                weight = Mathf.Clamp01(weight);
                Vector2 awayVectorNormalized = direction.normalized * -1;

                for (int i = 0; i < steering.Interest.Count; i++) {
                    float result = Vector2.Dot(awayVectorNormalized, steering.Directions[i]);
                    result = 1 - Mathf.Abs(result - .65f);
                    float value = result * weight * .5f;

                    if(value > steering.Interest[i])
                        steering.Interest[i] = value;
                }
            }
        }
        #endregion
    }
}
