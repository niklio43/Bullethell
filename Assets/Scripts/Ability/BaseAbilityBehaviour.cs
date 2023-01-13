using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.Abilities
{
    [System.Serializable]
    public abstract class BaseAbilityBehaviour : ScriptableObject
    {
        public virtual void Initialize(Ability ability, GameObject owner) { }
        public virtual void Uninitialize() { }
        public abstract void Perform(GameObject owner);
    }
}
