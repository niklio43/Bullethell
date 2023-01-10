using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Abilities;

[CreateAssetMenu(fileName = "AbilityProjectile", menuName = "Abilities/New Projectile Ability")]
public class AbilityProjectile : EmitterAbility
{
    Animator _animator;
    [SerializeField] AnimationClip _clip;

    public override void DoAbility()
    {
        _emitter.FireProjectile();

        _animator = Owner.GetComponent<Animator>();

        if (_animator == null || _clip == null) return;

        Owner.GetComponent<WeaponController>().PlayAnimation(_animator, _clip);
    }
}
