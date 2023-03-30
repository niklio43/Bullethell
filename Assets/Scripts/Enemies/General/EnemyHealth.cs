using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.EffectInterfaces;

namespace BulletHell.Enemies
{
    public class EnemyHealth : MonoBehaviour, IDamageable, IHealable
    {
        #region Private Fields
        [SerializeField] float _health;
        [SerializeField] float _maxHealth;
        #endregion

        #region Public Methods
        public void Damage(DamageValue Damage)
        {
            _health -= Damage.GetDamage();

            GetComponent<IStaggerable>().Stagger();

            DamagePopupManager.Instance.InsertIntoPool(Damage.GetDamage(), transform.position);

            if (_health <= 0) {
                GetComponent<IKillable>().Kill();
            }
        }

        public void Heal(float amount)
        {
            if(_health + amount > _maxHealth) { _health = _maxHealth; }
            else { _health = amount; }
        }
        #endregion
    }
}
