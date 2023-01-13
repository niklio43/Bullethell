using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.Abilities
{
    [CreateAssetMenu(fileName = "SwingAbilityBehaviour", menuName = "Abilities/Custom Behaviours/New Swing Behaviour")]
    public class SwingAbilityBehaviour : BaseAbilityBehaviour
    {
        public override void Perform(GameObject owner)
        {
            DetectColliders(1, owner);
        }

        //TODO Add additional functionality.
        public void DetectColliders(float damage, GameObject owner)
        {
            WeaponController weaponController = owner.GetComponent<WeaponController>();
            foreach (Collider2D collider in Physics2D.OverlapCircleAll(weaponController.CircleOrigin.position, weaponController.Radius))
            {
                Debug.Log(collider.name);
                GameObject other = collider.gameObject;
                if (other.CompareTag("Enemy"))
                {
                    other.GetComponent<Enemy>().TakeDamage(damage);
                }
            }
        }
    }
}