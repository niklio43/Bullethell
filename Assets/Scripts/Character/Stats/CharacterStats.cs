using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : ScriptableObject
{
    public Attribute[] attributes;

    public float Health;
    public float MaxHealth;
    public float MoveSpeed;

    public virtual void TakeDamage(float damage)
    {
        Health -= damage;
    }



}