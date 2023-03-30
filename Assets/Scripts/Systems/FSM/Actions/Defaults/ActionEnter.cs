using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.FiniteStateMachine
{
    public class ActionEnter : ActionBase
    {
        #region Private Fields
        private readonly Action<IAction> _enter;
        #endregion

        #region Public Fields
        public override string Name { get; set; } = "Enter";

        public ActionEnter(Action<IAction> enter)
        {
            _enter = enter;
        }
        #endregion

        #region Private Methods
        protected override void OnEnter()
        {
            _enter(this);
        }
        #endregion
    }
}
