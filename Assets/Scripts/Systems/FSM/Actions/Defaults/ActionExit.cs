using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.FiniteStateMachine
{
    public class ActionExit : ActionBase
    {
        #region Private Fields
        private readonly Action<IAction> _exit;
        #endregion

        #region Public Fields
        public override string Name { get; set; } = "Exit";

        public ActionExit(Action<IAction> exit)
        {
            _exit = exit;
        }
        #endregion

        #region Private Methods
        protected override void OnExit()
        {
            _exit(this);
        }
        #endregion
    }
}
