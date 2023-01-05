using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.FiniteStateMachine
{
    public class ActionExit : ActionBase
    {
        private readonly Action<IAction> _exit;

        public override string Name { get; set; } = "Exit";

        public ActionExit(Action<IAction> exit)
        {
            _exit = exit;
        }

        protected override void OnExit()
        {
            _exit(this);
        }
    }
}
