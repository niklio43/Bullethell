using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace BulletHell.Emitters.Projectiles
{
    public abstract class BaseProjectileBehaviour : ScriptableObject
    {
        ProjectileData _owner;

        public void SetOwner(ProjectileData owner) => _owner = owner;
        
        public void RemoveBehaviour()
        {
#if UNITY_EDITOR
            _owner.Behaviours.Remove(this);
            AssetDatabase.RemoveObjectFromAsset(this);
            AssetDatabase.SaveAssets();
#endif
        }

        public virtual void Initialize(Projectile owner, ProjectileData data) { }
        public virtual void UpdateBehaviour(Projectile projectile, ProjectileData data, float dt) { }
    }
}
