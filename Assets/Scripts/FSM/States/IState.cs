using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.FiniteStateMachine
{
    public interface IState
    {
        Enum Id { get; }
        List<IAction> Actions { get; }
        IFSM ParentFsm { get; }

        ITransition GetTransition(string name);
        void Enter();
        void Exit();
        void Update();
        void Transition(string id);
    }
}
