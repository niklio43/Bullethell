using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.StatusSystem
{
    public abstract class EffectBehaviour : ScriptableObject
    {
        public virtual void OnStart(StatusEffect statusEffect) { }
        public virtual void DoEffect(StatusEffect statusEffect) { }
        public virtual void OnExit(StatusEffect statusEffect) { }
    }
}