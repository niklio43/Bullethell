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
        public float weight = 0;

        public override void GetSteering(ContextMap danger, ContextMap interest, AgentSteering steering, DetectionData detectionData)
        {
            Transform transform = steering.transform;
            if(detectionData.PlayersCount == 0) { return; }


            List<Collider2D> target = detectionData.Players.OrderBy(n => Vector2.Distance(transform.position, n.transform.position)).ToList();
            Vector2 directionToTarget = target[0].transform.position - transform.position;

            for (int i = 0; i < interest.Count; i++) {
                float result = Vector2.Dot(directionToTarget.normalized, steering.Directions[i]);

                result *= weight;
                result = Mathf.Clamp01(result);

                if(result > interest[i])
                    interest[i] = result;
            }
        }
    }
}