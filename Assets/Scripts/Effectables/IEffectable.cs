using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.StatusSystem;

namespace BulletHell.EffectInterfaces
{
    public interface IEffectable
    {
        public void ApplyEffect(StatusEffect effect);
        public void RemoveEffect(StatusEffect effect);
    }
}
