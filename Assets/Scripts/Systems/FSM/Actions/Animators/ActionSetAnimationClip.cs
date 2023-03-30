using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.FiniteStateMachine
{
    public class ActionSetAnimationClip : ActionSetAnimatorVariableBase
    {
        #region Private Fields
        private readonly string _clipName;
        #endregion

        #region Public Fields
        public override string Name { get; set; } = "Set Animation Clip";

        public ActionSetAnimationClip(AnimationClip clip)
        {
            _clipName = clip.name;
        }

        public ActionSetAnimationClip(string clipName)
        {
            _clipName = clipName;
        }
        #endregion

        #region Private Methods
        protected override void OnEnter()
        {
            if(_animator == null) { return; }

            _animator?.Play(_clipName);
            _animator?.Update(Time.deltaTime);
        }
        #endregion
    }
}
