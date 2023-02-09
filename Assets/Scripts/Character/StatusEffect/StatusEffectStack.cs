using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.StatusSystem
{
    public class StatusEffectStack
    {
        readonly StatusEffect _owner;
        float _tickSpeed;
        float _nextTick;
        public float _lifeTime;
        public float CurrentLifeTime;

        public StatusEffectStack(StatusEffect owner, float lifeTime, float tickSpeed)
        {
            _owner = owner;
            _lifeTime = lifeTime;
            _tickSpeed = tickSpeed;
            ResetTimer();
        }

        public void ResetTimer()
        {
            CurrentLifeTime = _lifeTime;
            _nextTick = CurrentLifeTime;
        }

        public void Update(float dt)
        {
            CurrentLifeTime -= dt;
            if (CurrentLifeTime <= 0) {
                _owner.RemoveStack(this);
            }
            if (CurrentLifeTime <= _nextTick) {
                _owner.DoEffect();
                _nextTick = CurrentLifeTime - _tickSpeed;
            }
        }
    }
}
