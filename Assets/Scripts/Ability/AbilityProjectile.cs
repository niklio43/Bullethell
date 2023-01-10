using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Abilities;

[CreateAssetMenu(fileName = "AbilityProjectile", menuName = "Abilities/New Projectile Ability")]
public class AbilityProjectile : EmitterAbility
{

    public override void DoAbility(Weapon weapon, int abilityIndex)
    {
        _emitter.FireProjectile();

        if (weapon.AbilitySlot[abilityIndex].WeaponAttackAnimation == null) { Debug.Log("No Ability Animation"); return; }

        Owner.GetComponent<WeaponController>().PlayAnimation(abilityIndex, weapon);
    }
}
