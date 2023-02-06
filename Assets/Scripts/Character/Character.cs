using BulletHell.Stats;
using BulletHell.StatusSystem;
using UnityEngine;

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

    public delegate void OnStunDelegate(float duration);
    public OnStunDelegate OnStunEvent;

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

        if (Stats["Hp"].Value <= 0) {
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

    [ContextMenu("TESTSTUN")]
    public void TestStun()
    {
        Stun(2f);
    }


    public virtual void Heal(float amount)
    {
        Stats["Hp"].Value += amount;

        if (Stats["Hp"].Value > Stats["MaxHp"].Value) {
            Stats["Hp"].Value = Stats["MaxHp"].Value;
        }

        OnHealEvent?.Invoke(amount);
    }

}