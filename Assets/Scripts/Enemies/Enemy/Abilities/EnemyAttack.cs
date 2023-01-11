using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Abilities;

namespace BulletHell.Enemies
{
    public class EnemyAttack : MonoBehaviour
    {
        [SerializeField] List<AbilityProjectile> _attacks = new List<AbilityProjectile>();

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

        public void CastOrderedAbility(Transform target)
        {
            foreach (var ability in _attacks) {
                if (ability.CanCast()) {
                    ability.AimAtTarget(target);
                    ability.Activate();
                    break;
                }
            }
        }
    }
}
