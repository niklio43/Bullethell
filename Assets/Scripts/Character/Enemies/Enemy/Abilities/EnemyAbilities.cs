using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Abilities;

namespace BulletHell.Enemies
{
    public class EnemyAbilities : MonoBehaviour
    {
        [SerializeField] List<Ability> _abilities = new List<Ability>();

        private void Awake()
        {
            foreach (var ability in _abilities) {
                ability.Initialize(gameObject);
            }
        }

        private void Update()
        {
            foreach (Ability ability in _abilities) {
                ability.UpdateAbility(Time.deltaTime);
            }
        }

        public void UpdateAim(Transform target)
        {
            if(target == null) return;

            Vector3 aimDirection = (target.position - transform.position).normalized;
            float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0, 0, angle);
        }

        public void CastRandomAbility()
        {
            throw new System.NotImplementedException();
        }

        public bool CanAttack()
        {
            foreach (var ability in _abilities) {
                if (ability.CanCast()) return true;
            }

            return false;
        }

        [ContextMenu("TEST")]
        public void CastOrderedAbility()
        {
            foreach (var ability in _abilities) {
                if (ability.CanCast()) {
                    ability.Cast(); 
                    return;
                }
            }
        }
    }
}
