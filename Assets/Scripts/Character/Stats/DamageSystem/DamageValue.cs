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

public static class DamageCalculator
{
    public static float MitigateDamage(DamageInfo damage, CharacterStats stats)
    {
        float rawDamage = damage[DamageType.rawDamage] * (10 / (10 + stats.GetStatValue("Defense")));
        float fireDamage = damage[DamageType.fireDamage] - (damage[DamageType.fireDamage] * stats.GetStatValue("FireResistance"));

        return rawDamage + fireDamage;
    }
    
    public static DamageInfo CalculateDamage(DamageInfo damage, CharacterStats stats)
    {
        damage[DamageType.rawDamage] *= (1 + (stats.GetStatValue("Strength") / 10));

        return damage;
    }


}


public enum DamageType
{
    rawDamage,
    fireDamage,
    lightningDamage,
    trueDamage
}