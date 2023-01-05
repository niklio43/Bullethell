using BulletHell.Enemies.Detection;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BulletHell.Enemies.Steering
{
    [CreateAssetMenu(fileName = "SeekBehaviour", menuName = "Enemies/Steering/SeekBehaviour", order = 1)]
    public class SeekBehaviour : SteeringBehaviour
    {
        public float Weight = 0;

        public override void GetSteering(ContextMap danger, ContextMap interest, AgentSteering steering, DetectionData detectionData)
        {
            Transform transform = steering.transform;
            if(detectionData.Count("Player") == 0) { return; }

            List<EntityData> target = detectionData["Player"].OrderBy(n => Vector2.Distance(transform.position, n.transform.position)).ToList();
            Vector2 directionToTarget = target[0].transform.position - transform.position;

            for (int i = 0; i < interest.Count; i++) {
                float result = Vector2.Dot(directionToTarget.normalized, steering.Directions[i]);

                result *= Weight;
                result = Mathf.Clamp01(result);

                if(result > interest[i])
                    interest[i] = result;
            }
        }
    }
}