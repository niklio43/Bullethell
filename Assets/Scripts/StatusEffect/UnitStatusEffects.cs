using BulletHell.EffectInterfaces;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BulletHell.StatusSystem
{
    public class UnitStatusEffects : MonoBehaviour, IEffectable
    {
        #region Private Fields
        Dictionary<string, ActiveStatusEffect> _activeEffects;
        #endregion

        #region Public Fields
        public delegate void OnAppliedStatusEffectDelegate(ActiveStatusEffect effect);
        public event OnAppliedStatusEffectDelegate OnAppliedStatusEffect;
        public delegate void OnRemovedStatusEffectDelegate(ActiveStatusEffect effect);
        public event OnRemovedStatusEffectDelegate OnRemovedStatusEffect;
        #endregion

        #region Private Methods
        private void Awake()
        {
            _activeEffects = new Dictionary<string, ActiveStatusEffect>();
        }

        void AddNewStatusEffect(StatusEffect effect)
        {
            var data = new ActiveStatusEffect(effect);

            _activeEffects.Add(effect.Id, data);
            effect.Apply(this, gameObject, data);

            OnAppliedStatusEffect?.Invoke(data);
        }

        void AddStack(ActiveStatusEffect activeEffect)
        {
            switch (activeEffect.Effect.StackingBehaviour) {
                case StackingBehaviour.None:
                    break;
                case StackingBehaviour.Stackable:
                    if(activeEffect.CurrentStacks + 1 <= activeEffect.Effect.MaxStacks) {
                        activeEffect.CurrentStacks++;
                    }
                    activeEffect.ResetDuration();
                    break;
                case StackingBehaviour.Resettable:
                    activeEffect.ResetDuration();
                    break;
            }
        }
        #endregion

        #region Public Methods
        public void ApplyEffect(StatusEffect effect)
        {
            if (_activeEffects.ContainsKey(effect.Id)) {
                AddStack(_activeEffects[effect.Id]);
            }
            else {
                AddNewStatusEffect(effect);
            }
        }

        public void RemoveEffect(StatusEffect effect)
        {
            if (!_activeEffects.ContainsKey(effect.Id)) { return; }
            OnRemovedStatusEffect?.Invoke(_activeEffects[effect.Id]);

            _activeEffects.Remove(effect.Id);
        }
        #endregion
    }
}
