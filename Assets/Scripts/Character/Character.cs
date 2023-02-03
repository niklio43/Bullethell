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
    public StatusEffectData StatusEffect;

    public UnityEvent<float> OnTakeDamageEvent;
    public UnityEvent OnDeathEvent;
    public UnityEvent<float> OnHealEvent;
    public UnityEvent<float> OnStunEvent;

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

    public void Stun(float duration)
    {
        OnStunEvent?.Invoke(duration);
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