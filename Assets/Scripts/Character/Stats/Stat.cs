using System.Collections.Generic;
using UnityEngine;


namespace BulletHell.Stats
{
    [System.Serializable]
    public class Stat
    {
        public string Name = "NoName";

        Dictionary<string, StatModifier> _modifiers = new Dictionary<string, StatModifier>();

        [SerializeField] float _value;
        public float Value { get => _value; set { _value = value; OnValueChanged?.Invoke(); } }

        public delegate void OnValueChangedDelegate();
        public OnValueChangedDelegate OnValueChanged;

        public float Get()
        {
            float total = _value;
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

            OnValueChanged?.Invoke();
        }

        public void AddModifier(StatModifier modifier)
        {
            if (_modifiers.ContainsKey(modifier.Id)) { 
                Debug.LogWarning("A modifier with the same id is already applied to the stat."); 
                return; 
            }

            _modifiers.Add(modifier.Id, modifier);

            OnValueChanged?.Invoke();
        }

        public void RemoveModifier(StatModifier modifier)
        {
            _modifiers.Remove(modifier.Id);
            OnValueChanged?.Invoke();
        }

    }
}
