using System.Collections.Generic;
using UnityEngine;
using BulletHell.Emitters.Projectiles.Behaviours;

namespace BulletHell.Emitters.Projectiles
{
    [CreateAssetMenu(fileName = "ProjectileData", menuName = "Emitters/TESt")]
    public class ProjectileData : ScriptableObject
    {
        [Header("General")]
        public new string name = "NewProjectileData";
        public float Scale = 1;

        [Header("Sprite")]
        public Sprite Sprite;
        public Color Birth = Color.white;
        public Color MidLife = Color.white;
        public Color Death = Color.white;

        [Header("Behaviour")]
        public float Speed;
        public float MaxSpeed;
        public float Acceleration;
        public List<BaseProjectileBehaviour> Behaviours = new List<BaseProjectileBehaviour>();

        [Header("Collision")]
        public Bounds Collider;
        public List<string> CollisionTags = new List<string>();

    }
}
