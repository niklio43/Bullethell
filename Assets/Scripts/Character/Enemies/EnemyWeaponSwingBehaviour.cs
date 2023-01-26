using BulletHell.Emitters;
using BulletHell.Stats;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.Abilities
{
    [CreateAssetMenu(fileName = "EnemySwingBehaviour", menuName = "Abilities/Custom Behaviours/New Enemy Swing Behaviour")]
    public class EnemyWeaponSwingBehaviour : BaseAbilityBehaviour
    {
        [SerializeField] EmitterData _emitterData;
        [SerializeField] List<DamageValue> _damageValues;

        Emitter _emitterObject;

        int _animationIndex = 0;

        protected override void Initialize()
        {
            _emitterObject = new GameObject($"{_ability.GetName()} (Emitter)").AddComponent<Emitter>();
            _emitterObject.transform.SetParent(_ability.Owner.GetComponent<Enemy>().Aim.transform);
            _emitterObject.transform.localPosition = Vector3.zero;
            _emitterObject.Data = _emitterData;
            _emitterObject.AutoFire = false;
        }
        public override void Uninitialize()
        {
            MonoBehaviour.Destroy(_emitterObject.gameObject);
        }

        protected override void Perform()
        {
            DamageInfo damage = new DamageInfo(_damageValues);

            if (_ability.Owner.TryGetComponent(out Character character)) {
                damage = DamageCalculator.CalculateDamage(damage, character.Stats);
            
            }

            _emitterObject.SetDamage(damage);
            _emitterObject.FireProjectile();
        }

        protected override void WhenCompletedChannel()
        {
            PlayAnimation();
        }

        protected override void PlayAnimation()
        {
            if (_animationIndex >= _animations.Count) _animationIndex = 0;
            _animations[_animationIndex].Play();
            _animationIndex++;
        }
    }
}
