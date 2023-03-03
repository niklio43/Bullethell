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
            if (!entityRoot.TryGetComponent(out IDamageable damageEntity)) { effectContainer.RemoveEffect(this); }


            while (runTimeData.Duration > 0) {
                yield return new WaitForSeconds(TickSpeed);
                runTimeData.Duration -= TickSpeed;
                runTimeData.Tick();
                DamageValue damage = new DamageValue(Damage.GetDamageType(), Damage.GetDamage() * runTimeData.CurrentStacks);
                damageEntity.Damage(damage);
            }

            effectContainer.RemoveEffect(this);
        }
    }
}
