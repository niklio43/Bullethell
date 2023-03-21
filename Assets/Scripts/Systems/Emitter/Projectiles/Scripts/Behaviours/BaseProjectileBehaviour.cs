using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace BulletHell.Emitters.Projectiles
{
    public abstract class BaseProjectileBehaviour : ScriptableObject
    {
        public abstract string Id();
        public virtual void Initialize(Projectile owner, ProjectileData data) { }
        public virtual void UpdateBehaviour(Projectile owner, ProjectileData data, float dt) { }
    }
}
