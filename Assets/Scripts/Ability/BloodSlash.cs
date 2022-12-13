using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Abilities;

[CreateAssetMenu(fileName = "BloodSlash", menuName = "Abilities/BloodSlash")]
public class BloodSlash : EmitterAbility
{
    public override void DoAbility()
    {
        _emitter.FireProjectile();
    }
}
