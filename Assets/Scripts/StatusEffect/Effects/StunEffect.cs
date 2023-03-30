using BulletHell.EffectInterfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.StatusSystem
{
    [CreateAssetMenu(fileName = "StunEffect", menuName = "Status Effects/New Stun Effect")]
    public class StunEffect : StatusEffect
    {
        #region Private Methods
        protected override IEnumerator DoEffect(UnitStatusEffects effectContainer, GameObject entityRoot, ActiveStatusEffect runTimeData)
        {
            if (!entityRoot.TryGetComponent(out IStunable stunEntity)) { effectContainer.RemoveEffect(this); }

            stunEntity.Stun();
            while(runTimeData.Duration > 0) {
                yield return new WaitForEndOfFrame();
                runTimeData.Duration -= Time.deltaTime;
            }

            stunEntity.ExitStun();
            effectContainer.RemoveEffect(this);
        }
        #endregion
    }
}
