using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.Emitters
{
    [System.Serializable]
    public class ModifierController
    {
        public List<EmitterModifier> Modifiers;
        public EmitterModifier this[int i] => Modifiers[i];
        public List<EmitterModifier> GetModifiers() => Modifiers;
        public int Count => Modifiers.Count;


        public EmitterModifier AddModifier()
        {
            EmitterModifier newModifier = new EmitterModifier();
            Modifiers.Add(newModifier);

            return newModifier;
        }

        public void DeleteModifier(int index)
        {
            Modifiers.RemoveAt(index);
        }
    }
}
