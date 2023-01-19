using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using BulletHell.VFX;
using BulletHell.Stats;

namespace BulletHell.Abilities
{
    [CreateAssetMenu(fileName = "SwingAbilityBehaviour", menuName = "Abilities/Custom Behaviours/New Swing Behaviour")]
    public class SwingAbilityBehaviour : BaseAbilityBehaviour
    {
        [Header("VFX")]
        [SerializeField] VisualEffectAsset _vfx;
        public override void Perform(GameObject owner, GameObject host)
        {
            DetectColliders(1, owner);
        }

        //TODO Add additional functionality.
        public void DetectColliders(float damage, GameObject owner)
        {
            WeaponController weaponController = owner.GetComponent<WeaponController>();

            BulletHell.VFX.VFXManager.PlayBurst(_vfx, Vector3.zero, owner.transform, new VFXAttribute[] { new VFXFloat("Angle", weaponController.transform.rotation.eulerAngles.z) });

            foreach (Collider2D collider in Physics2D.OverlapCircleAll(weaponController.CircleOrigin.position, weaponController.Radius))
            {
                Debug.Log(collider.name);
                GameObject other = collider.gameObject;
                if (other.CompareTag("Enemy"))
                {
                    List<DamageValue> _damage = new List<DamageValue>() { new DamageValue(DamageType.rawDamage, 1) };

                    other.GetComponent<Enemy>().TakeDamage(new DamageInfo(_damage));
                }
            }
        }
    }
}