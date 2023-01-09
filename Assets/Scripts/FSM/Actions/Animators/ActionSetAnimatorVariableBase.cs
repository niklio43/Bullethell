using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.FiniteStateMachine
{
    public class ActionSetAnimatorVariableBase : ActionBase
    {
        protected Animator _animator;

        protected override void OnInit()
        {
            _animator = ParentState.ParentFsm.Owner.GetComponent<Animator>();
        }
    }
}
