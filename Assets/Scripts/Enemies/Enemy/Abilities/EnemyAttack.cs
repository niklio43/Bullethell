using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Abilities;

namespace BulletHell.Enemies
{
    public class EnemyAttack : MonoBehaviour
    {
        /*[SerializeField] List<AbilityProjectile> _attacks = new List<AbilityProjectile>();

        private void Awake()
        {
            foreach (var ability in _attacks) {
                ability.Initialize(gameObject);
            }
        }

        private void Update()
        {
            foreach (Ability ability in _attacks) {
                ability.UpdateAbility(Time.deltaTime);
            }
        }

        public void CastRandomAbility()
        {
            throw new System.NotImplementedException();
        }

        public bool CanAttack()
        {
            foreach (var ability in _attacks) {
                if (ability.CanCast()) {
                    return true;
                }
            }

            return false;
        }

        public void CastOrderedAbility(Transform target)
        {
            foreach (var ability in _attacks) {
                if (ability.CanCast()) {
                    Debug.Log("Annika");
                    ability.AimAtTarget(target);
                    ability.Activate();
                    break;
                }
            }
        }*/
    }
}
