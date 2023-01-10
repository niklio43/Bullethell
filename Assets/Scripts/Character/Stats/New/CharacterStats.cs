using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.Stats
{
    [System.Serializable]
    public class CharacterStats
    {
        public Stat this[string key] { get { return Stats[key]; } }


        [SerializeField] List<Stat> _stats = new List<Stat>();
        public Dictionary<string, Stat> Stats = new Dictionary<string, Stat>();

        public void Initialize()
        {
            foreach (Stat stat in _stats) {
                Stats.Add(stat.Name, stat);
            }
        }

        public void AddModifierToStat(StatModifier modifier)
        {
            Stats[modifier.Stat].AddModifier(modifier);
        }

        public void RemoveModifierFromStat(StatModifier modifier)
        {
            Stats[modifier.Stat].RemoveModifier(modifier);
        }

        public void UpdateList()
        {
            foreach (Stat stat in Stats.Values) {
                foreach (Stat _stat in _stats) {
                    if(_stat.Name == stat.Name) {
                        _stat.Value = stat.Get();
                    }
                }
            }
        }

    }
}
