using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.Emitters
{
    [CreateAssetMenu(fileName = "EmitterData", menuName = "Emitters/EmitterData", order = 1)]
    public class EmitterData : ScriptableObject
    {
        [Header("General")]
        [Range(0, 5000)] public int Delay = 1000;
        [Range(0, 1000)] public int MaxProjectiles = 10;

        [Header("Projectile")]
        public ProjectileData ProjectileData;
        public float TimeToLive = 5;
        [Range(0.01f, 100f)] public float BaseSpeed = 1;

        [Header("Emission")]
        [Range(1, 40)] public int EmitterPoints = 1;
        [Range(-180, 180)] public float CenterRotation = 0;
        [Range(-180, 180)] public float Pitch = 0;
        [Range(0, 10)] public float Offset = 0;
        [Range(-180, 180)] public float Spread = 0;

        Projectile _projectilePrefab;
        public Projectile ProjectilePrefab
        {
            get {
                if (_projectilePrefab == null) {
                    _projectilePrefab = Resources.Load<Projectile>("EmitterProjectile");
                }

                return _projectilePrefab;
            }
        }

        public ModifierController Modifiers;
    }
}
