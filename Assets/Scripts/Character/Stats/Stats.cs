using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell;
using UnityEngine.Rendering;

namespace BulletHell.Stats
{
    [System.Serializable]
    public class Stats
    {
        public Stat this[string key] { get {
                if (!_stats.ContainsKey(key)) return default(Stat);
                return _stats[key]; 
            } 
        }

        public float GetStatValue(string key)
        {
            if (!_stats.ContainsKey(key)) return 0f;
            return _stats[key].Value;
        }

        [SerializeField] List<Stat> _statsList = new List<Stat>();

        public Dictionary<string, Stat> _stats = new Dictionary<string, Stat>();

        public void TranslateListToDictionary()
        {
            _stats = new Dictionary<string, Stat>();

            foreach (Stat stat in _statsList) {
                _stats.Add(stat.Name, Utilities.Copy(stat));
            }
        }

        public void AddModifierToStat(StatModifier modifier, float time)
        {
            _stats[modifier.Stat].AddModifier(modifier, time);
        }

        public void AddModifierToStat(StatModifier modifier)
        {
            _stats[modifier.Stat].AddModifier(modifier);
        }

        public void RemoveModifierFromStat(StatModifier modifier)
        {
            _stats[modifier.Stat].RemoveModifier(modifier);
        }
    }
}

