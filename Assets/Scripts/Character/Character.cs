using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Stats;


public class Character : MonoBehaviour
{
    public CharacterStats Stats;

    public virtual void TakeDamage(float amount)
    {
        Stats["Hp"].Value += amount;

        if(Stats["Hp"].Value <= 0) {
            OnDeath();
        }
    }

    public virtual void Heal(float amount)
    {
        Stats["Hp"].Value += amount;

        if(Stats["Hp"].Value > Stats["MaxHp"].Value) {
            Stats["Hp"].Value = Stats["MaxHp"].Value;
        }
    }

    public virtual void OnDeath()
    {
        throw new System.NotImplementedException();
    }
}