using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.EffectInterfaces;

namespace BulletHell.StatusSystem
{
    [CreateAssetMenu(fileName = "DOTEffect", menuName = "Status Effects/New DOT Effect")]
    public class DOTEffect : StatusEffect
    {
        public float TickSpeed;
        public DamageValue Damage;

        protected override IEnumerator DoEffect(UnitStatusEffects effectContainer, GameObject entityRoot, ActiveStatusEffect runTimeData)
        {
            while(runTimeData.Duration > 0) {
                yield return new WaitForSeconds(TickSpeed);
                runTimeData.Duration -= TickSpeed;
                runTimeData.Tick();
                DamageValue damage = new DamageValue(Damage.GetDamageType(), Damage.GetDamage() * runTimeData.CurrentStacks);
                entityRoot.GetComponent<IDamageable>().Damage(damage);
            }

            effectContainer.RemoveEffect(this);
        }
    }
}
