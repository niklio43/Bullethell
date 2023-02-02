using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Stats;

namespace BulletHell.StatusSystem
{
    public class BasicDamageEffect : EffectBehaviour
    {
        public override void DoEffect(StatusEffect statusEffect)
        {
            DamageHandler.SendDamage(statusEffect.Sender, statusEffect.Reciever, statusEffect.DamageInfo);
            Debug.Log("test");
        }
    }
}