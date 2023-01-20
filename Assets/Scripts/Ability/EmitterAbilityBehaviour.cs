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

        public override void Initialize(Ability ability, GameObject owner, GameObject host)
        {
            _emitterObjects = new List<Emitter>();
            _emitterObjects.Clear();

            foreach (EmitterData emitterData in _emitters) {
                Emitter emitter = new GameObject($"{ability.GetName()} (Emitter)").AddComponent<Emitter>();
                emitter.transform.SetParent(host.transform);
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

        public override void Perform(GameObject owner, GameObject host)
        {
            Character character = owner.GetComponent<Character>();

            DamageInfo damage = new DamageInfo(_damageValues);
            damage = DamageCalculator.CalculateDamage(damage, character.Stats);

            foreach (Emitter emitter in _emitterObjects) {
                emitter.SetDamage(damage);
                emitter.FireProjectile();
            }
        }
    }
}
