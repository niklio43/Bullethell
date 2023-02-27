using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Abilities;
using UnityEngine.VFX;
using BulletHell.StatusSystem;

namespace BulletHell.Enemies.AbilityBehaviours
{
    public class StompBehaviour : EmitterAbilityBehaviour
    {
        [Header("Damage")]
        [SerializeField] List<DamageValue> Damage;
        [SerializeField] List<StatusEffect> StatusEffects;
        [Header("Other")]
        [SerializeField] float timeBetween = .1f;
        [SerializeField] float timeForDetonation = 1f;
        [SerializeField] VisualEffectAsset VFX;

        protected override void Initialize()
        {
            _emitterObject = new Emitter(_ability.Host, _emitterData);
        }




    }
}
