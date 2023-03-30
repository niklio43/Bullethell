using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.Emitters.Projectiles.Behaviours
{
    public class GravityBehaviour : BaseProjectileBehaviour
    {
        #region Private Fields
        [SerializeField] Vector2 _point;
        [SerializeField] float amplitude;
        #endregion

        #region Public Fields
        public override string Id() => "02";
        #endregion

        #region Public Methods
        public override void UpdateBehaviour(Projectile owner, ProjectileData data, float dt)
        {
            Vector3 dir = (Vector3)_point - owner.transform.position;
            owner.Velocity += (dir.normalized * amplitude) * dt;
        }
        #endregion
    }
}