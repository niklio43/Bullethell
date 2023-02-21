using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace BulletHell.Emitters.Projectiles
{
    public abstract class BaseProjectileBehaviour : ScriptableObject
    {
        [HideInInspector] public ProjectileData Owner;

        public void SetOwner(ProjectileData owner) => Owner = owner;

        public abstract string Id();


        public void RemoveBehaviour()
        {
#if UNITY_EDITOR
            Owner.Behaviours.Remove(this);
            AssetDatabase.RemoveObjectFromAsset(this);
            AssetDatabase.SaveAssets();
#endif
        }

        public virtual void Initialize(Projectile owner, ProjectileData data) { }
        public virtual void UpdateBehaviour(Projectile projectile, ProjectileData data, float dt) { }
    }
}
