using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.FiniteStateMachine
{
    public class ActionSetAnimationClip : ActionSetAnimatorVariableBase
    {
        private readonly string _clipName;

        public override string Name { get; set; } = "Set Animation Clip";

        public ActionSetAnimationClip(AnimationClip clip)
        {
            _clipName = clip.name;
        }

        public ActionSetAnimationClip(string clipName)
        {
            _clipName = clipName;
        }

        protected override void OnEnter()
        {
            _animator.Play(_clipName);
        }
    }
}
