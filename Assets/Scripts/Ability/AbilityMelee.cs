using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Abilities;

[CreateAssetMenu(fileName = "AbilityMelee", menuName = "Abilities/New Melee Ability")]
public class AbilityMelee : Ability
{
    GameObject _owner;

    public override void Initialize(GameObject owner)
    {
        base.Initialize(owner);
        _owner = owner;
    }

    public override void DoAbility()
    {
        _owner.GetComponent<WeaponController>().DetectColliders();
    }
}
