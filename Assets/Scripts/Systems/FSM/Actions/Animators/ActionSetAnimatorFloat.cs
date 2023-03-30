using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.FiniteStateMachine
{
    public class ActionSetAnimatorFloat : ActionSetAnimatorVariableBase
    {
        #region Private Fields
        private readonly string _paramName;
        private readonly float _value;
        #endregion

        #region Public Fields
        public override string Name { get; set; } = "Set Animator Float";

        public ActionSetAnimatorFloat(string paramName, float value)
        {
            _paramName = paramName;
            _value = value;
        }
        #endregion

        #region Private Methods
        protected override void OnEnter()
        {
            _animator.SetFloat(_paramName, _value);
        }
        #endregion
    }
}