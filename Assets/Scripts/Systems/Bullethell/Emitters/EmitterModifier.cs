using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.Emitters
{
    [System.Serializable]
    public class EmitterModifier
    {
        public bool enabled = true;

        public int Factor;
        public int Count;

        string name;

        [Header("Projectile Modifiers")]
        public ProjectileData ProjectileData;
        [Range(0.1f, 5f)] public float SpeedMultiplier = 1;

        [Header("Emission Modifiers")]
        [Range(-180, 180)] public float Pitch = 0;
        [Range(0, 10)] public float Offset = 0;
        [Range(-180, 180)] public float Spread = 0;
    }
}