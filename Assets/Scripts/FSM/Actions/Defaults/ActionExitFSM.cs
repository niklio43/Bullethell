using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BulletHell.FiniteStateMachine {
    public class ActionExitFSM : ActionBase
    {
        protected override void OnEnter()
        {
            ParentState.ParentFsm.Exit();
        }
    }
}
