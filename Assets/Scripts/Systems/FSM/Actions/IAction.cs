using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BulletHell.FiniteStateMachine
{
    public interface IAction
    {
        string Name { get; }
        IState ParentState { get; set; }

        void Enter();
        void Exit();
        void Update();
        void Transition(string id);
    }
}
