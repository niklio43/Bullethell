using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Stats;

[System.Serializable]
public class DamageValue
{
    public DamageSender senderType;
    public DamageType Type;
    public float Value;

    public DamageValue(DamageSender senderType, DamageType type, float value)
    {
        Type = type;
        Value = value;
    }
}

public enum DamageSender
{
    Projectile,
    Melee,
    Environment,
    Unblockable
}


public enum DamageType
{
    rawDamage,
    fireDamage,
    lightningDamage,
    trueDamage
}