using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.EffectInterfaces;
using BulletHell;
using BulletHell.GameEventSystem;

public class PlayerResources : MonoBehaviour, IDamageable, IHealable, IKillable
{
    #region Public Fields
    public float Health;
    public float MaxHealth;
    public int Stamina;
    public float MaxStamina;

    [Header("Events")]
    public SOGameEvent OnHealthChanged;
    public SOGameEvent OnMaxHealthChanged;
    public SOGameEvent OnStaminaChanged;
    public SOGameEvent OnMaxStaminaChanged;
    #endregion

    void Start()
    {
        OnHealthChanged.Raise(this, Health);
        OnMaxHealthChanged.Raise(this, MaxHealth);
        OnStaminaChanged.Raise(this, Stamina);
        OnStaminaChanged.Raise(this, MaxStamina);
    }

    #region Public Methods
    public void Damage(DamageValue Damage)
    {
        Health -= Damage.GetDamage();
        OnHealthChanged.Raise(this, Health);
        if(Health <= 0) { Kill(); }
    }

    public void UseStamina(int amount)
    {
        Stamina -= amount;
        OnStaminaChanged.Raise(this, Stamina);
        Invoke("AddStamina", 1);
    }

    public void AddStamina()
    {
        Stamina++;
        OnStaminaChanged.Raise(this, Stamina);
    }

    public void Heal(float amount)
    {
        if(Health + amount > MaxHealth)
        {
            Health = MaxHealth;
            return;
        }
        Health += amount;
        OnHealthChanged.Raise(this, Health);
    }

    public void ModifyMaxHealth(float amount)
    {
        MaxHealth += amount;
        OnMaxHealthChanged.Raise(this, MaxHealth);
    }

    public void Kill()
    {
        throw new System.NotImplementedException();
    }
    #endregion
}
