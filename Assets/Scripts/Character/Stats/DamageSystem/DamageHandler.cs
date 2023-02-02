using BulletHell.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DamageHandler
{
    public static void SendDamage(GameObject sender, Character receiver, DamageInfo damage)
    {
        float finilizedDamage = MitigateDamage(damage, receiver.Stats);
        receiver.TakeDamage(finilizedDamage);

        Debug.Log($"{sender.name} dealt {finilizedDamage} damage to {receiver.name}!");
    }

    public static float MitigateDamage(DamageInfo damage, Stats stats)
    {
        float rawDamage = damage[DamageType.rawDamage] * (10 / (10 + stats.GetStatValue("Defense")));
        float fireDamage = damage[DamageType.fireDamage] - (damage[DamageType.fireDamage] * stats.GetStatValue("FireResistance"));

        return rawDamage + fireDamage;
    }

    public static DamageInfo CalculateDamage(DamageInfo damage, Stats stats)
    {
        damage[DamageType.rawDamage] *= (1 + (stats.GetStatValue("Strength") / 10));

        return damage;
    }
}
