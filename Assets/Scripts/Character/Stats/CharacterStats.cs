using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : ScriptableObject
{
    public Attribute[] attributes;

    float _health;
    float _maxHealth;
    float _moveSpeed;
    int _dashDistance;

    public float Health { get { return _health; } set { _health = value; } }
    public float MaxHealth { get { return _maxHealth; } set { _maxHealth = value; } }
    public float MoveSpeed { get { return _moveSpeed; } set { _moveSpeed = value; } }
    public int DashDistance { get { return _dashDistance; } set { _dashDistance = value; } }
}