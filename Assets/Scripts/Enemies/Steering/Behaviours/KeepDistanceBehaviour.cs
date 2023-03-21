using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.Enemies.Steering
{
    [CreateAssetMenu(fileName = "KeepDistanceBehaviour", menuName = "Enemies/Steering/KeepDistanceBehaviour", order = 1)]
    public class KeepDistanceBehaviour : SteeringBehaviour
    {
        public float Weight = 0;
        public float Clamp = 0;

        public override void GetSteering(AgentSteering steering, EnemyMovement movement)
        {
            Transform transform = movement.transform;
            Enemy enemy = movement.Enemy;

            if(enemy.Target == null) { return; }

            float distance = Vector2.Distance(enemy.Target.position, transform.position);

            if(distance > enemy.PreferredDistance || !enemy.TargetInLineOfSight(transform.position)) { return; }

            float finalizedWeight = (1 - (distance / enemy.PreferredDistance));

            if(finalizedWeight < Clamp) { return; }

            Vector2 awayVector = transform.position - enemy.Target.position;

            for (int i = 0; i < steering.Directions.Length; i++) {
                float result = Vector2.Dot(awayVector, steering.Directions[i]);
                result = result * Weight * finalizedWeight;

                result = Mathf.Clamp01(result);

                if(result > steering.Interest[i])
                    steering.Interest[i] = result;
            }
        }
    }
}
