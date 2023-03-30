using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.FiniteStateMachine
{
    public class ActionSetAnimatorVariableBase : ActionBase
    {
        #region Private Fields
        protected Animator _animator;
        #endregion

        #region Private Methods
        protected override void OnInit()
        {
            _animator = ParentState.ParentFsm.Owner.GetComponentInChildren<Animator>();
        }
        #endregion
    }
}
