using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BulletHell.Emitters.Projectiles.Behaviours
{
    public class TrackBehaviour : BaseProjectileBehaviour
    {
        #region Private Fields
        [SerializeField] float TrackRadius;
        [SerializeField] float TrackIntensity;
        #endregion

        #region Public Fields
        public override string Id() => "03";
        #endregion

        #region Public Methods
        public override void Initialize(Projectile owner, ProjectileData data)
        {
            owner.StartCoroutine(Track(owner, data));
        }
        #endregion

        #region Private Methods
        IEnumerator Track(Projectile owner, ProjectileData data)
        {
            Transform currentTarget = null;
            while (currentTarget == null) {
                yield return new WaitForSeconds(.1f);
                currentTarget = FindTarget(owner);
            }

            while (true) {
                yield return new WaitForFixedUpdate();
                Vector3 dir = currentTarget.position - owner.transform.position;
                owner.Velocity += (dir * TrackIntensity) * Time.fixedDeltaTime;
            }

        }

        Transform FindTarget(Projectile owner)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(owner.transform.position, TrackRadius, 1 << LayerMask.NameToLayer("Entity"));
            if(colliders.Length == 0) { return null; }

            Transform closestCollider = null;
            float closestDist = TrackRadius;

            foreach (Collider2D collider in colliders) {
                if(!owner.Data.CollisionTags.Contains(collider.tag)) { continue; }
                float dist = Vector2.Distance(owner.transform.position, collider.transform.position);

                if (dist <= closestDist) {
                    closestDist = dist;
                    closestCollider = collider.transform;
                }
            }

            return closestCollider;
        }
        #endregion
    }
}
