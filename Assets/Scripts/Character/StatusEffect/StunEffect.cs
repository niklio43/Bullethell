using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Stats;

namespace BulletHell.StatusSystem
{
    [CreateAssetMenu(fileName = "Stun Effect", menuName = "Status Effect/New Stun Effect")]
    public class StunEffect : EffectBehaviour
    {
        public override void DoEffect(StatusEffect statusEffect)
        {
            statusEffect.Reciever.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        }

        public override void OnExit(StatusEffect statusEffect)
        {
            statusEffect.Reciever.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            statusEffect.Reciever.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }
}