using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.EffectInterfaces;
using BulletHell;

public class PlayerResources : MonoBehaviour, IDamageable, IHealable, IKillable
{
    #region Public Fields
    public float Health;
    public float MaxHealth;
    public int Stamina;
    public float MaxStamina;

    public delegate void OnHealthChangedDelegate();
    public event OnHealthChangedDelegate OnHealthChanged;

    public delegate void OnStaminaChangedDelegate();
    public event OnStaminaChangedDelegate OnStaminaChanged;
    #endregion

    #region Public Methods
    public void Damage(DamageValue Damage)
    {
        Health -= Damage.GetDamage();
        OnHealthChanged?.Invoke();
        if(Health <= 0) { Kill(); }
    }

    public void UseStamina(int amount)
    {
        Stamina -= amount;
        OnStaminaChanged?.Invoke();
        Invoke("AddStamina", 1);
    }

    public void AddStamina()
    {
        Stamina++;
        OnStaminaChanged?.Invoke();
    }

    public void Heal(float amount)
    {
        if(Health + amount > MaxHealth)
        {
            Health = MaxHealth;
            return;
        }
        Health += amount;
        OnHealthChanged?.Invoke();
    }

    public void ModifyMaxHealth(float amount)
    {
        MaxHealth += amount;
        OnHealthChanged?.Invoke();
    }

    public void Kill()
    {
        throw new System.NotImplementedException();
    }
    #endregion
}
