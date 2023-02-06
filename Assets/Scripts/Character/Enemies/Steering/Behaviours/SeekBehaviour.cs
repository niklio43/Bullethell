using BulletHell.Enemies.Detection;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BulletHell.Enemies.Steering
{
    [CreateAssetMenu(fileName = "SeekBehaviour", menuName = "Enemies/Steering/Seek Behaviour", order = 1)]
    public class SeekBehaviour : SteeringBehaviour
    {
        public float Weight = 0;
        public float Margin = 0;

        public override void GetSteering(AgentSteering steering, EnemyMovement movement)
        {
            Transform transform = movement.transform;
            Collider2D collider = movement.Collider;
            Enemy enemy = movement.Enemy;

            if(enemy.Target == null) { return; }

            float distance = Vector2.Distance(enemy.Target.position, transform.position);

            Vector2 towardsVector = enemy.Target.position - transform.position;

            float distanceWeight = Mathf.Clamp01(distance / 3);
            int towards = 0;

            if(Mathf.Abs(distance - enemy.PreferredDistance) > Margin) {
                towards = (int)Mathf.Sign(distance - enemy.PreferredDistance);
            }

            for (int i = 0; i < steering.Directions.Length; i++) {
                float result = Vector2.Dot(towardsVector.normalized * towards, steering.Directions[i]);
                result = result * Weight * distanceWeight;

                result = Mathf.Clamp01(result);

                if(result > steering.Interest[i])
                    steering.Interest[i] = result;
            }
        }
    }
}