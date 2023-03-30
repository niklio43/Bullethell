using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.FiniteStateMachine
{
    public class ActionUpdate : ActionBase
    {
        #region Private Fields
        private readonly Action<IAction> _update;
        #endregion

        #region Public Fields
        public override string Name { get; set; } = "Update";

        public ActionUpdate(Action<IAction> update)
        {
            _update = update;
        }
        #endregion

        #region Private Methods
        protected override void OnUpdate()
        {
            _update(this);
        }
        #endregion
    }
}
