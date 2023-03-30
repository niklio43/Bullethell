using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.StatusSystem
{
    public class ActiveStatusEffect
    {
        #region Public Fields
        public int CurrentStacks = 0;
        public float Duration = 0;
        public readonly StatusEffect Effect;

        public delegate void OnTickDelegate();
        public event OnTickDelegate OnTick;
        #endregion

        #region Public Methods
        public ActiveStatusEffect(StatusEffect statusEffect)
        {
            Effect = statusEffect;
            CurrentStacks = 1;
            Duration = Effect.Duration;
        }

        public float GetRemainingLifePercentage() => Duration / Effect.Duration;
        public void ResetDuration() => Duration = Effect.Duration;
        public void Tick() => OnTick?.Invoke();
        #endregion
    }
}
