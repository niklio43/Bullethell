using BulletHell.Emitters;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Player;

namespace BulletHell.Abilities
{
    [CreateAssetMenu(fileName = "EnemySwingBehaviour", menuName = "Abilities/Custom Behaviours/New Enemy Swing Behaviour")]
    public class EnemyWeaponSwingBehaviour : EmitterAbilityBehaviour
    {
        int _animationIndex = 0;

        protected override void Initialize()
        {
            GameObject emitterOwner = _ability.Host;
            if (_ability.Owner.TryGetComponent(out Enemy enemy))
                emitterOwner = enemy.Aim.gameObject;
            else if (_ability.Owner.TryGetComponent(out PlayerController player))
                emitterOwner = player.Aim.gameObject;

            _emitterObject = new Emitter(emitterOwner, _emitterData);
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
