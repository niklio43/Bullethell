using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Stats;


public class Character : MonoBehaviour
{
    public CharacterStats stats;

    public virtual void TakeDamage(float amount)
    {
        stats["Hp"].Value += amount;

        if(stats["Hp"].Value <= 0) {
            OnDeath();
        }
    }

    public virtual void Heal(float amount)
    {
        stats["Hp"].Value += amount;

        if(stats["Hp"].Value > stats["MaxHp"].Value) {
            stats["Hp"].Value = stats["MaxHp"].Value;
        }
    }

    public virtual void OnDeath()
    {
        throw new System.NotImplementedException();
    }
}