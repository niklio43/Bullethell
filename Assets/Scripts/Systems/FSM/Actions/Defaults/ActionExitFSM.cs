using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BulletHell.FiniteStateMachine {
    public class ActionExitFSM : ActionBase
    {
        #region Private Methods
        protected override void OnEnter()
        {
            ParentState.ParentFsm.Exit();
        }
        #endregion
    }
}
