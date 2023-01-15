using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.FiniteStateMachine
{
    public class ActionRunFSM : ActionBase
    {
        private IFSM _fsm;
        private string _exitTransition;
        private bool _triggerExit;

        public override string Name => "Run FSM";

        public ActionRunFSM(string exitTransition, IFSM fsm)
        {
            _fsm = fsm;
            _exitTransition = exitTransition;
        }

        protected override void OnInit()
        {
            _fsm.EventExit.AddListener(() => _triggerExit = true);
        }

        protected override void OnEnter()
        {
            _fsm.Reset();
            _triggerExit = false;
        }

        protected override void OnUpdate()
        {
            if(_triggerExit) {
                Transition(_exitTransition);
                return;
            }

            _fsm.Update();
        }
    }
}
