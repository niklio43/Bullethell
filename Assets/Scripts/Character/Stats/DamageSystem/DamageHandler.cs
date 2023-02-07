using BulletHell.Stats;
using BulletHell.StatusSystem;
using System.Collections.Generic;
using UnityEngine;

public static class DamageHandler
{
    public static void SendDamage(Character sender, Character receiver, DamageInfo damage)
    {
        float finilizedDamage = MitigateDamage(damage, receiver.Stats);
        receiver.TakeDamage(finilizedDamage);

        SendStatusEffects(sender, receiver, damage.StatusEffects);

        Debug.Log($"{sender.name} dealt {finilizedDamage} damage to {receiver.name}!");
    }

    public static void SendStatusEffects(Character sender, Character receiver, List<StatusEffect> statusEffects)
    {
        if (statusEffects == null) { return; }
        foreach (StatusEffect effect in statusEffects) {
            effect.SetReceiver(receiver);
            receiver.StatusEffect.AddEffect(effect);
        }
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
