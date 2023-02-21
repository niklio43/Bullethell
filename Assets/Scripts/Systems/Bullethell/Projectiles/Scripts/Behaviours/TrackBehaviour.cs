using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BulletHell.Emitters.Projectiles.Behaviours
{
    public class TrackBehaviour : BaseProjectileBehaviour
    {
        [SerializeField] float TrackIntensity;

        public override string Id() => "03";

        public override void UpdateBehaviour(Projectile projectile, ProjectileData data, float dt)
        {
            if(projectile.Target == null) return;

            Vector2 dir = projectile.Target.position - projectile.transform.position;
            projectile.Velocity += (dir * TrackIntensity) * dt;
        }
    }
}
