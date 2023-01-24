using BulletHell.VFX;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace BulletHell.Abilities
{
    [CreateAssetMenu(fileName = "EnemySwingBehaviour", menuName = "Abilities/Custom Behaviours/New Enemy Swing Behaviour")]
    public class EnemyWeaponSwingBehaviour : BaseAbilityBehaviour
    {
        [SerializeField] VisualEffectAsset _vfx;

        protected override void Perform()
        {
            BulletHell.VFX.VFXManager.PlayBurst(_vfx, Vector3.zero, _ability.Owner.transform, new VFXAttribute[] { new VFXFloat("Angle", _ability.Host.transform.rotation.eulerAngles.z) });
        }
    }
}
