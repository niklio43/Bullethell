using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.StatusSystem
{
    public class ActiveStatusEffect
    {
        public int CurrentStacks = 0;
        public float Duration = 0;
        public readonly StatusEffect Effect;

        public delegate void OnTickDelegate();
        public event OnTickDelegate OnTick;

        public ActiveStatusEffect(StatusEffect statusEffect)
        {
            Effect = statusEffect;
            CurrentStacks = 1;
            Duration = Effect.Duration;
        }

        public float GetRemainingLifePercentage() => Duration / Effect.Duration;
        public void ResetDuration() => Duration = Effect.Duration;
        public void Tick() => OnTick?.Invoke();
    }
}
