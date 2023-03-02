using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell
{
    [System.Serializable]
    public class DamageValue
    {
        [SerializeField] DamageType _damageType = DamageType.Projectile;
        [SerializeField] float _value;

        public DamageType GetDamageType() => _damageType;
        public float GetDamage() => _value;

        public DamageValue(DamageType damageType, float value)
        {
            _damageType = damageType;
            _value = value;
        }

    }

    public enum DamageType
    {
        Projectile,
        Melee,
        Enviroment,
        Unblockable
    }
}