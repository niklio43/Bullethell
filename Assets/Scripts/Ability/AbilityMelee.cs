using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Abilities;

[CreateAssetMenu(fileName = "AbilityMelee", menuName = "Abilities/New Melee Ability")]
public class AbilityMelee : Ability
{
    GameObject _owner;
    [SerializeField] List<AnimationClip> _clip;

    public override void Initialize(GameObject owner)
    {
        base.Initialize(owner);
        _owner = owner;
    }

    public override void DoAbility()
    {
        _owner.GetComponent<WeaponController>().DetectColliders();

        if (_clip == null) return;

        _owner.GetComponent<WeaponController>().PlayAnimation(_clip);
    }
}
