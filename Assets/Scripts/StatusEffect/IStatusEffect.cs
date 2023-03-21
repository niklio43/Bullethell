using BulletHell.EffectInterfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.StatusSystem
{
    public interface IStatusEffect
    {
        public void Apply(UnitStatusEffects effectContainer, GameObject entityRoot, ActiveStatusEffect runtimeEffect);
    }
}
