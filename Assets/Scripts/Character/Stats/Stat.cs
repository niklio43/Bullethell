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

        public void AddModifier(StatModifier modifier, float time)
        {
            if (_modifiers.ContainsKey(modifier.Id)) {
                Debug.LogWarning("A modifier with the same id is already applied to the stat.");
                return;
            }

            AddModifier(modifier);
            MonoInstance.GetInstance().Invoke(() => { RemoveModifier(modifier); }, time);
        }

        public void AddModifier(StatModifier modifier)
        {
            if (_modifiers.ContainsKey(modifier.Id)) { 
                Debug.LogWarning("A modifier with the same id is already applied to the stat."); 
                return; 
            }

            _modifiers.Add(modifier.Id, modifier);
        }

        public void RemoveModifier(StatModifier modifier)
        {
            _modifiers.Remove(modifier.Id);
        }

    }
}
