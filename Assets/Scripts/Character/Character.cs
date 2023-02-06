using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Stats;
using BulletHell;
using BulletHell.StatusSystem;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    public Stats Stats;
    [HideInInspector] public StatusEffectData StatusEffect;

    public delegate void OnTakeDamageDelegate(float amount);
    public OnTakeDamageDelegate OnTakeDamageEvent;

    public delegate void OnHealDelegate(float amount);
    public OnHealDelegate OnHealEvent;

    public delegate void OnDeathDelegate();
    public OnDeathDelegate OnDeathEvent;

    public delegate void OnStunDelegate();
    public OnStunDelegate OnStunEvent;

    public delegate void OnExitStunDelegate();
    public OnStunDelegate OnExitStunEvent;

    private void Awake()
    {
        Stats.TranslateListToDictionary();
    }

    private void Update()
    {
        StatusEffect.UpdateEffects(Time.deltaTime);
    }

    public virtual void TakeDamage(float damage)
    {
        Stats["Hp"].Value -= damage;

        OnTakeDamageEvent?.Invoke(damage);

        if (Stats["Hp"].Value <= 0)
        {
            OnDeath();
        }
    }

    public void OnDeath()
    {
        OnDeathEvent?.Invoke();
    }

    public void Stun()
    {
        OnStunEvent?.Invoke();
    }

    public void ExitStun()
    {
        OnExitStunEvent?.Invoke();
    }

    public virtual void Heal(float amount)
    {
        Stats["Hp"].Value += amount;

        if (Stats["Hp"].Value > Stats["MaxHp"].Value)
        {
            Stats["Hp"].Value = Stats["MaxHp"].Value;
        }

        OnHealEvent?.Invoke(amount);
    }

}