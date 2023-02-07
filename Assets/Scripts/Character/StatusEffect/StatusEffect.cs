using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Stats;
using UnityEngine.VFX;

namespace BulletHell.StatusSystem
{
    [System.Serializable]
    public class StatusEffect
    {
        [Header("General")]
        public string Name;
        public Sprite Icon;
        public DamageInfo DamageInfo;

        [Header("Stacking Behaviour")]
        [SerializeField] float maxStacks = 5;
        [SerializeField] StatusBehaviour _stackingBehaviour = StatusBehaviour.None;

        [Header("Time")]
        [SerializeField] float tickSpeed = 1;
        [SerializeField] float lifeTime = 1;

        [Header("Behaviours")]
        [SerializeField] List<EffectBehaviour> _behaviours = new List<EffectBehaviour>();
        [SerializeField] List<DamageValue> _damageValues = new List<DamageValue>();

        public enum StatusBehaviour
        {
            None,
            Stackable,
            Resettable
        }
        public Character Sender { get; private set; }
        public Character Reciever { get; private set; }

        List<StatusEffectStack> _stacks;

        public void Initialize(Character sender)
        {
            Sender = sender;
            _stacks = new List<StatusEffectStack>();
            DamageInfo = new DamageInfo(_damageValues);
            DamageInfo = DamageHandler.CalculateDamage(DamageInfo, sender.Stats);
        }

        public void SetReceiver(Character reciever) => Reciever = reciever;

        public void ApplyEffect()
        {
            if (_stacks.Count == 0)
                AddStack();
            else {
                switch (_stackingBehaviour) {
                    case StatusBehaviour.None:
                        break;
                    case StatusBehaviour.Stackable:
                        if (_stacks.Count - 1 < maxStacks) {
                            AddStack();
                        }
                        break;
                    case StatusBehaviour.Resettable:
                        _stacks[0].ResetTimer();
                        break;
                }
            }
        }

        public void AddStack()
        {
            _stacks.Add(new StatusEffectStack(this, lifeTime, tickSpeed));
            foreach (EffectBehaviour effect in _behaviours) {
                effect.OnStart(this);
            }
        }

        public void RemoveStack(StatusEffectStack stack)
        {
            _stacks.Remove(stack);
            foreach (EffectBehaviour effect in _behaviours) {
                effect.OnExit(this);
            }
            if(_stacks.Count == 0) {
                Reciever.StatusEffect.RemoveEffect(this);
            }
        }

        public void DoEffect()
        {
            foreach (EffectBehaviour behaviour in _behaviours) {
                behaviour.DoEffect(this);
            }
        }


        public void Update(float dt)
        {
            for (int i = 0; i < _stacks.Count; i++) {
                _stacks[i].Update(dt);
            }
        }


        public class StatusEffectStack
        {
            readonly StatusEffect _owner;
            float _tickSpeed;
            float _nextTick;
            float _lifeTime;
            float _currentLifeTime;

            public StatusEffectStack(StatusEffect owner, float lifeTime, float tickSpeed)
            {
                _owner = owner;
                _lifeTime = lifeTime;
                ResetTimer();
            }

            public void ResetTimer()
            {
                _currentLifeTime = _lifeTime;
                _nextTick = 0;
            }

            public void Update(float dt)
            {
                _currentLifeTime -= dt;
                if(_currentLifeTime <= 0) {
                    _owner.RemoveStack(this);
                }
                if(_currentLifeTime >= _nextTick) {
                    _owner.DoEffect();
                    _nextTick = _currentLifeTime + _tickSpeed;
                }
            }
        }
    }
}