using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.EffectInterfaces;

namespace BulletHell.Enemies
{
    public class EnemyHealth : MonoBehaviour, IDamageable, IHealable
    {
        [SerializeField] float _health;
        [SerializeField] float _maxHealth;

        public void Damage(DamageValue Damage)
        {
            _health -= Damage.GetDamage();

            GetComponent<IStaggerable>().Stagger();

            if(_health <= 0) {
                GetComponent<IKillable>().Kill();
            }
        }

        public void Heal(float amount)
        {
            if(_health + amount > _maxHealth) { _health = _maxHealth; }
            else { _health = amount; }
        }
    }
}
