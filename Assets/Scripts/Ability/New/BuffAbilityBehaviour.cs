using BulletHell.Stats;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.Abilities.New
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

        public override void Perform(GameObject owner)
        {
            CharacterStats stats = owner.GetComponent<CharacterStats>();

            foreach (StatModifier buff in _buffs) {
                switch (_durationPolicy) {
                    case DurationPolicy.Instant:
                        stats.AddModifierToStat(buff, 0);
                        break;
                    case DurationPolicy.HasDuration:
                        stats.AddModifierToStat(buff, _duration);
                        break;
                    case DurationPolicy.Permanent:
                        stats.AddModifierToStat(buff);
                        break;
                }
            }
        }

    }
}
