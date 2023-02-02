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
        public string Name;
        public Sprite Icon;
        public float TickSpeed = 1;
        public float Lifetime;
        public bool Stackable;
        public DamageInfo DamageInfo;
        public List<EffectBehaviour> Behaviours = new List<EffectBehaviour>();
        [HideInInspector] public Character Sender, Reciever;

        [SerializeField] List<DamageValue> _damageValues = new List<DamageValue>();
        float _nextTickTime = 0f;
        float _timer = 0f;

        public void Initialize(Character sender)
        {
            Sender = sender;
            DamageInfo = new DamageInfo(_damageValues);
            DamageInfo = DamageHandler.CalculateDamage(DamageInfo, sender.Stats);
        }

        public void ApplyEffect(Character reciever)
        {
            Reciever = reciever;
            Reciever.StatusEffect.AddEffect(this);
            foreach (EffectBehaviour effect in Behaviours) {
                effect.OnStart(this);
            }
        }

        public void EndEffect()
        {
            _timer = 0;
            _nextTickTime = 0;
            foreach (EffectBehaviour effect in Behaviours) {
                effect.OnExit(this);
            }
            Reciever.StatusEffect.RemoveEffect(this);
        }


        public void UpdateStatus(float dt)
        {
            _timer += dt;
            if (_timer >= Lifetime) { EndEffect(); }

            if (_timer > _nextTickTime)
            {
                _nextTickTime += TickSpeed;
                foreach(EffectBehaviour behaviour in Behaviours)
                {
                    behaviour.DoEffect(this);
                }
            }
        }
    }
}