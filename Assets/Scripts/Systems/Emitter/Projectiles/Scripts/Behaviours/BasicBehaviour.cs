using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.Emitters.Projectiles.Behaviours
{
    public class BasicBehaviour : BaseProjectileBehaviour
    {
        #region Public Fields
        public override string Id() => "01";
        #endregion

        #region Public Methods
        public override void Initialize(Projectile owner, ProjectileData data)
        {
            owner.Velocity *= data.Speed;
        }

        public override void UpdateBehaviour(Projectile owner, ProjectileData data, float dt)
        {
            owner.Velocity *= (1 + data.Acceleration * dt);
        }
        #endregion
    }
}
