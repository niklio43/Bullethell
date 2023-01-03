using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Abilities;

[CreateAssetMenu(fileName = "AbilityMelee", menuName = "Abilities/New Melee Ability")]
public class AbilityMelee : Ability
{
    WeaponController _weaponController;

    public override void Initialize(WeaponController weaponController)
    {
        base.Initialize(weaponController);
        _weaponController = weaponController;
    }

    public override void DoAbility()
    {
        Debug.Log("Swing");
        _weaponController.DetectColliders();
    }

}
