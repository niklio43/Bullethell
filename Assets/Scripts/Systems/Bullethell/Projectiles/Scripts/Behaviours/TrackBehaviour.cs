using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BulletHell.Emitters.Projectiles.Behaviours
{
    public class TrackBehaviour : BaseProjectileBehaviour
    {
        [SerializeField] float TrackIntensity;

        public override string Id() => "03";

        public override void UpdateBehaviour(Projectile owner, ProjectileData data, float dt)
        {
            if(owner.Target == null) return;

            Vector3 dir = owner.Target.position - owner.transform.position;
            owner.Velocity += (dir * TrackIntensity) * dt;
        }
    }
}
