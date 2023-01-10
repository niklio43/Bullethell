using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.Stats
{
    public class StatModifier
    {
        public string Stat;
        public string Id;

        public ModifierType type = ModifierType.Add;
        public float Value;

        public enum ModifierType
        {
            Add,
            Mult
        }
    }
}
