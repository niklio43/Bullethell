using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.Stats
{
    [System.Serializable]
    public class StatModifier
    {
        public string Stat;
        public string Id;
        public ModifierType type = ModifierType.Add;
        
        [SerializeField] float _value;
        public float Value { get => _value; set => _value = value; }
        
        public enum ModifierType
        {
            Add,
            Mult
        }
    }
}
