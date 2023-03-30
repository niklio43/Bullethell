using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.FiniteStateMachine
{
    public class ActionSetAnimatorTrigger : ActionSetAnimatorVariableBase
    {
        #region Private Fields
        private readonly string _paramName;
        #endregion

        #region Public Fields
        public override string Name { get; set; } = "Set Animator Trigger";

        public ActionSetAnimatorTrigger(string paramName)
        {
            _paramName = paramName;
        }
        #endregion

        #region Private Methods
        protected override void OnEnter()
        {
            _animator.SetTrigger(_paramName);
        }
        #endregion
    }
}
