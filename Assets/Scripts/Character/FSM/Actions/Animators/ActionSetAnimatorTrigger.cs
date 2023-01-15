using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.FiniteStateMachine
{
    public class ActionSetAnimatorTrigger : ActionSetAnimatorVariableBase
    {
        private readonly string _paramName;

        public override string Name { get; set; } = "Set Animator Trigger";

        public ActionSetAnimatorTrigger(string paramName)
        {
            _paramName = paramName;
        }

        protected override void OnEnter()
        {
            _animator.SetTrigger(_paramName);
        }
    }
}
