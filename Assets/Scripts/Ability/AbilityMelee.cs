using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Abilities;

[CreateAssetMenu(fileName = "AbilityMelee", menuName = "Abilities/New Melee Ability")]
public class AbilityMelee : Ability
{
    GameObject _owner;
    Animator _animator;
    [SerializeField] AnimationClip _clip;

    public override void Initialize(GameObject owner)
    {
        base.Initialize(owner);
        _owner = owner;
        _animator = _owner.GetComponent<Animator>();
    }

    public override void DoAbility()
    {
        _owner.GetComponent<WeaponController>().DetectColliders();

        if (_animator == null || _clip == null) return;

        _owner.GetComponent<WeaponController>().PlayAnimation(_animator, _clip);
    }
}
