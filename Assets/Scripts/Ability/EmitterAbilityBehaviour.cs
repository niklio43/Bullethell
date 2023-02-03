using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Emitters;
using BulletHell.Stats;
using BulletHell.StatusSystem;

namespace BulletHell.Abilities
{
    [CreateAssetMenu(fileName = "EmitterAbilityBehaviour", menuName = "Abilities/New Emitter Ability Behaviour")]
    public class EmitterAbilityBehaviour : BaseAbilityBehaviour
    {
        [SerializeField] protected EmitterData _emitterData;
        protected Emitter _emitterObject;
        [SerializeField] protected List<DamageValue> _damageValues;
        [SerializeField] protected List<StatusEffect> _statusEffects;
        protected override void Initialize()
        {
            _emitterObject = new Emitter(_ability.Host, _emitterData);
        }

        protected override void OnUpdate(float dt)
        {
            _emitterObject.UpdateEmitter(dt);
        }

        public override void Uninitialize()
        {
            if(_emitterObject == null) return;
            _emitterObject.Uninitialize();
            _emitterObject = null;
        }

        protected override void Perform()
        {
            DamageInfo damage = new DamageInfo(_damageValues);

            if (_ability.Owner.TryGetComponent(out Character character)) {
                foreach (StatusEffect effect in _statusEffects) {
                    effect.Initialize(character);
                }
                damage = DamageHandler.CalculateDamage(damage, character.Stats);
            }

            _emitterObject.FireProjectile(character, damage, _statusEffects);
        }
    }
}
