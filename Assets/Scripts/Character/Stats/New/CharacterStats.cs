using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.Stats
{
    public class CharacterStats
    {
        public Stat this[string key] { get { return this[key]; } }


        [SerializeField] List<Stat> _stats = new List<Stat>();
        public Dictionary<string, Stat> Stats = new Dictionary<string, Stat>();

        public void Awake()
        {
            foreach (Stat stat in _stats) {
                Stats.Add(stat.Name, stat);
            }
        }

        public void AddModifierToStat(StatModifier modifier)
        {
            Stats[modifier.Stat].AddModifier(modifier);
        }
    }
}
