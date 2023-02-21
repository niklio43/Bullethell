using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.Emitters.Projectiles.Behaviours
{
    public class GravityBehaviour : BaseProjectileBehaviour
    {
        [SerializeField] Vector2 _point;
        [SerializeField] float amplitude;

        public override string Id() => "02";

        public override void UpdateBehaviour(Projectile projectile, ProjectileData data, float dt)
        {
            Vector2 dir = (Vector3)_point - projectile.transform.position;
            projectile.Velocity += (dir.normalized * amplitude) * dt;


        }
    }
}