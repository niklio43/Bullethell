using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Stats;

namespace BulletHell.StatusSystem
{
    public class StunEffect : EffectBehaviour
    {
        public override void DoEffect(StatusEffect statusEffect)
        {
            statusEffect.Reciever.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            Debug.Log("test");
        }
    }
}