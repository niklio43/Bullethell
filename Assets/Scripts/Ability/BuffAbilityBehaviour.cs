using BulletHell.Stats;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.Abilities
{
    [CreateAssetMenu(fileName = "BuffAbilityBehaviour", menuName = "Abilities/New Buff Ability Behaviour")]
    public class BuffAbilityBehaviour : BaseAbilityBehaviour
    {
        enum DurationPolicy
        {
            Instant,
            HasDuration,
            Permanent
        }

        [Header("Duration")]
        [SerializeField] DurationPolicy _durationPolicy;
        [SerializeField] float _duration;

        [SerializeField] List<StatModifier> _buffs;

        public override void Perform(GameObject owner, GameObject host)
        {
            if(!owner.TryGetComponent(out Character stats)) { Debug.LogWarning($"Could not apply buff. {owner.name} does not inherit from Character."); }

            Stats.Stats characterStats = stats.Stats;

            foreach (StatModifier buff in _buffs) {
                switch (_durationPolicy) {
                    case DurationPolicy.Instant:
                        characterStats.AddModifierToStat(buff, 0);
                        break;
                    case DurationPolicy.HasDuration:
                        characterStats.AddModifierToStat(buff, _duration);
                        break;
                    case DurationPolicy.Permanent:
                        characterStats.AddModifierToStat(buff);
                        break;
                }
            }
        }

    }
}
