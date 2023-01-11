using System.Collections.Generic;
using UnityEngine;


namespace BulletHell.Stats
{
    [System.Serializable]
    public class Stat
    {
        public string Name = "NoName";
        public float Value;
        Dictionary<string, StatModifier> _modifiers = new Dictionary<string, StatModifier>();

        public float Get()
        {
            float total = Value;
            float multiplier = 0;

            foreach (StatModifier modifier in _modifiers.Values) {
                if (modifier.type == StatModifier.ModifierType.Add) total += modifier.Value;
                else multiplier += modifier.Value;
            }

            return total + (total * multiplier);
        }

        public void AddModifier(StatModifier modifier)
        {
            _modifiers.Add(modifier.Id, modifier);
        }

        public void RemoveModifier(StatModifier modifier)
        {
            _modifiers.Remove(modifier.Id);
        }
    }
}
