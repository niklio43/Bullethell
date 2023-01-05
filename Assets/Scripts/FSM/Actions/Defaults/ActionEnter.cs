using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.FiniteStateMachine
{
    public class ActionEnter : ActionBase
    {
        private readonly Action<IAction> _enter;

        public override string Name { get; set; } = "Enter";

        public ActionEnter(Action<IAction> enter)
        {
            _enter = enter;
        }

        protected override void OnEnter()
        {
            _enter(this);
        }
    }
}
