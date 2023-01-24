using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.Abilities {
    [System.Serializable]
    public class AbilityAnimation
    {
        [SerializeField] AnimationType _animationHost = AnimationType.Owner;
        [SerializeField] AnimationClip _clip;
        Animator _ownerAnimator, _hostAnimator;

        public enum AnimationType {
            Owner,
            Host,
            Both
        }
        public void Initialize(Ability ability)
        {
            _ownerAnimator = ability.Owner.GetComponent<Animator>();
            _hostAnimator = ability.Host.GetComponent<Animator>();
        }

        public void Play()
        {
            switch (_animationHost) {
                case AnimationType.Owner:
                    _ownerAnimator?.Play(_clip.name);
                    break;
                case AnimationType.Host:
                    _hostAnimator?.Play(_clip.name);
                    break;
                case AnimationType.Both:
                    _ownerAnimator?.Play(_clip.name);
                    _hostAnimator?.Play(_clip.name);
                    break;
                default:
                    break;
            }
        }
    }
}
