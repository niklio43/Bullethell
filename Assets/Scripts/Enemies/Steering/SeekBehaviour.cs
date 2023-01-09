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

        public override void GetSteering(AgentSteering steering, Enemy enemy)
        {
            Transform transform = steering.Owner.transform;
            if(enemy.Target == null) { return; }

            float distance = Vector2.Distance(enemy.Target.position, transform.position);

            Vector2 towardsVector = enemy.Target.position - transform.position;

            int towards = 0;

            if(Mathf.Abs(distance - enemy.Stats.PreferredDistance) > Margin) {
                towards = (int)Mathf.Sign(distance - enemy.Stats.PreferredDistance);
            }

            for (int i = 0; i < steering.Directions.Length; i++) {
                float result = Vector2.Dot(towardsVector.normalized * towards, steering.Directions[i]);
                result *= Weight;

                result = Mathf.Clamp01(result);

                if(result > steering.Interest[i])
                    steering.Interest[i] = result;
            }
        }
    }
}