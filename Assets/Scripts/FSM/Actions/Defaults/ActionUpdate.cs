using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.FiniteStateMachine
{
    public class ActionUpdate : ActionBase
    {
        private readonly Action<IAction> _update;

        public override string Name { get; set; } = "Update";

        public ActionUpdate(Action<IAction> update)
        {
            _update = update;
        }
        protected override void OnUpdate()
        {
            _update(this);
        }
    }
}
