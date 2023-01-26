using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Emitters;
using BulletHell.Stats;

namespace BulletHell.Abilities
{
    [CreateAssetMenu(fileName = "EmitterAbilityBehaviour", menuName = "Abilities/New Emitter Ability Behaviour")]
    public class EmitterAbilityBehaviour : BaseAbilityBehaviour
    {
        [SerializeField] List<EmitterData> _emitters;
        [SerializeField] List<DamageValue> _damageValues;
        List<Emitter> _emitterObjects;

        protected override void Initialize()
        {
            _emitterObjects = new List<Emitter>();
            _emitterObjects.Clear();

            foreach (EmitterData emitterData in _emitters) {
                Emitter emitter = new GameObject($"{_ability.GetName()} (Emitter)").AddComponent<Emitter>();
                emitter.transform.SetParent(_ability.Host.transform);
                emitter.transform.localPosition = Vector3.zero;
                emitter.Data = emitterData;
                emitter.AutoFire = false;
                _emitterObjects.Add(emitter);
            }
        }
        public override void Uninitialize()
        {
            foreach (Emitter emitter in _emitterObjects) {
                MonoBehaviour.Destroy(emitter.gameObject);
            }
        }

        protected override void Perform()
        {
            DamageInfo damage = new DamageInfo(_damageValues);

            if (_ability.Owner.TryGetComponent(out Character character)) {
                damage = DamageCalculator.CalculateDamage(damage, character.Stats);
            }

            foreach (Emitter emitter in _emitterObjects) {
                emitter.SetDamage(damage);
                emitter.FireProjectile();
            }
        }
    }
}
