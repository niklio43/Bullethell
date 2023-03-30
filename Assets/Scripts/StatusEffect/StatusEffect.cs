using BulletHell.EffectInterfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace BulletHell.StatusSystem
{
    [System.Serializable]
    public abstract class StatusEffect : ScriptableObject, IStatusEffect
    {
        #region Public Fields
        [Header("General")]
        public Sprite Icon;
        public string Id;
        [TextArea(1, 3)] public string Description;
        public float Duration;

        [Header("Stacking Behaviour")]
        public StackingBehaviour StackingBehaviour = StackingBehaviour.None;
        public float MaxStacks = 5;
        #endregion

        #region Public Methods
        public void Apply(UnitStatusEffects effectContainer, GameObject entityRoot, ActiveStatusEffect runTimeData)
        {
            effectContainer.StartCoroutine(DoEffect(effectContainer, entityRoot, runTimeData));
        }
        #endregion

        #region Private Methods
        protected abstract IEnumerator DoEffect(UnitStatusEffects effectContainer, GameObject entityRoot, ActiveStatusEffect runTimeData);
        #endregion
    }
}