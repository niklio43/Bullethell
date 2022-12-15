using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.Enemies.Detection {

    public class AgentFOV : MonoBehaviour
    {
        [Header("FOV")]
        [Range(0, 360)] public float Angle = 45;
        public float Radius = 2;
        public float innerRadius = 1;

        public Collider2D[] Detect(LayerMask mask, LayerMask obstacleMask)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, Radius, mask);

            List<Collider2D> visable = new List<Collider2D>();

            foreach (Collider2D item in colliders) {

                if (TargetInView(item.transform, obstacleMask)) {
                    visable.Add(item);
                }
            }

            return visable.ToArray();
        }

        public bool TargetInView(Transform target, LayerMask obstacleMask)
        {
            float dist = Vector2.Distance(transform.position, target.position);
            if(dist > Radius) { return false; }

            Vector2 direction = (target.position - transform.position).normalized;

            if(dist < innerRadius || Vector2.Angle(transform.up, direction) < Angle / 2) {
                if(!Physics.Raycast(transform.position, direction, dist, obstacleMask)) {
                    return true;
                }
            }

            return false;
        }
    }
}