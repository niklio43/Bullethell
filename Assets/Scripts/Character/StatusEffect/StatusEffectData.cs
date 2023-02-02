using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using BulletHell.Stats;
using BulletHell;
using System.Linq;

namespace BulletHell.StatusSystem
{
    [System.Serializable]
    public class StatusEffectData
    {
        public Dictionary<string, StatusEffect> _status = new Dictionary<string, StatusEffect>();

        public StatusEffect this[string key]
        {
            get
            {
                if (!_status.ContainsKey(key)) return default(StatusEffect);
                return _status[key];
            }
        }

        public StatusEffect GetEffect(string key)
        {
            if (!_status.ContainsKey(key)) return null;
            return _status[key];
        }

        public void AddEffect(StatusEffect statusEffect)
        {
            if (_status.ContainsKey(statusEffect.Name)) { _status[statusEffect.Name].Multiple(); return; }

            _status.Add(statusEffect.Name, statusEffect);
        }

        public void RemoveEffect(StatusEffect statusEffect)
        {
            _status.Remove(statusEffect.Name);
        }

        public void UpdateEffects(float dt)
        {
            for (int i = 0; i < _status.Count; i++) {
                _status.ElementAt(i).Value.UpdateStatus(dt);
            }
        }
    }
}