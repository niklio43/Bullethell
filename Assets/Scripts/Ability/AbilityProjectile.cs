using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Abilities;

[CreateAssetMenu(fileName = "AbilityProjectile", menuName = "Abilities/New Projectile Ability")]
public class AbilityProjectile : EmitterAbility
{
    [SerializeField] List<AnimationClip> _clip;

    public override void DoAbility()
    {
        _emitter.FireProjectile();

        if (_clip == null) return;
        if(Owner.TryGetComponent(out WeaponController weaponController))
        weaponController.PlayAnimation(_clip);
    }

    public void AimAtTarget(Transform target)
    {
        Vector2 direction = target.position - Owner.transform.position;
        AimAtDirection(direction);
    }

}
