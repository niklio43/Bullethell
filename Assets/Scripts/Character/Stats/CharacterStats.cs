using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell;
using UnityEngine.Rendering;

namespace BulletHell.Stats
{
    [System.Serializable]
    public class CharacterStats
    {
        public Stat this[string key] { get { return _stats[key]; } }

        [SerializeField] List<Stat> _statsList = new List<Stat>();

        [System.Serializable]
        public class StatDictionary : SerializedDictionary<string, Stat> { };
        StatDictionary _stats;
        GameObject _owner;

        public void Initialize(GameObject owner)
        {
            _owner = owner;
        }

        public void TranslateListToDictionary()
        {
            _stats = new StatDictionary();

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

