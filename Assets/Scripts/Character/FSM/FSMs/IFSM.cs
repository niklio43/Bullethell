using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BulletHell.FiniteStateMachine
{
    public interface IFSM
    {
        IState GetState(Enum id);
        void AddState(IState state);
        void SetState(Enum id);

        GameObject Owner { get; }
        IState CurrentState { get; }
        IState DefaultState { get; set; }
        UnityEvent EventExit { get; }

        void Update();
        void Reset();
        void Exit();
    }
}
