using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.StatusSystem
{
    public abstract class EffectBehaviour : ScriptableObject
    {
        public abstract void DoEffect(StatusEffect statusEffect);
    }
}