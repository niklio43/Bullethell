using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BulletHell.Emitters.Projectiles
{
    public abstract class BaseProjectileBehaviour : ScriptableObject
    {
        public virtual void Initialize(Projectile owner, ProjectileData data) { }
        public virtual void UpdateBehaviour(Projectile projectile, ProjectileData data, float dt) { }
    }
}
