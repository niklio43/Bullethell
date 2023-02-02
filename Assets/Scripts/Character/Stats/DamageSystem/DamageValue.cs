using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Stats;

[System.Serializable]
public class DamageValue
{
    public DamageType Type;
    public float Value;

    public DamageValue(DamageType type, float value)
    {
        Type = type;
        Value = value;
    }
}


public enum DamageType
{
    rawDamage,
    fireDamage,
    lightningDamage,
    trueDamage
}