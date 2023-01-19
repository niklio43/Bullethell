using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.Stats
{
    public class DamageInfo
    {
        public float this[DamageType type]
        {
            get {
                if (!_damages.ContainsKey(type)) return 0f;
                return _damages[type];
            }
            set {
                _damages[type] = value;
            }
        }
        public Dictionary<DamageType, float> _damages;
        public DamageInfo(List<DamageValue> damageList)
        {
            _damages = new Dictionary<DamageType, float>();


            foreach (DamageValue damageValue in damageList) {
                if (!_damages.ContainsKey(damageValue.Type)) {
                    _damages.Add(damageValue.Type, 0f);
                }

                _damages[damageValue.Type] += damageValue.Value;
            }
        }

        public float GetDamage(DamageType type)
        {
            if (!_damages.ContainsKey(type)) return 0f;
            return _damages[type];
        }
    }
}
