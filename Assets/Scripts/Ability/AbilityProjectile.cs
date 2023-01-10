using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Abilities;

[CreateAssetMenu(fileName = "AbilityProjectile", menuName = "Abilities/New Projectile Ability")]
public class AbilityProjectile : EmitterAbility
{

    public override void DoAbility()
    {
        _emitter.FireProjectile();


    }
}
