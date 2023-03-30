using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.FiniteStateMachine
{
    public class State : IState
    {
        #region Private Fields
        private readonly Dictionary<string, ITransition> _transitions = new Dictionary<string, ITransition>();
        #endregion

        #region Public Fields
        public Enum Id { get; }
        public List<IAction> Actions { get; } = new List<IAction>();
        public IFSM ParentFsm { get; }

        public State (IFSM fsm, Enum id)
        {
            ParentFsm = fsm;
            Id = id;
        }
        #endregion

        #region Public Methods
        public void AddTransition(ITransition transition)
        {
            _transitions[transition.Name] = transition;
        }

        public ITransition GetTransition(string name)
        {
            ITransition result;
            _transitions.TryGetValue(name, out result);

            return result;
        }

        public void Enter()
        {
            foreach (var action in Actions) {
                action.Enter();
            }
        }

        public void Exit()
        {
            foreach (var action in Actions) {
                action.Exit();
            }
        }

        public void Update()
        {
            foreach (var action in Actions) {
                action.Update();
            }
        }

        public void Transition(string id)
        {
            var transition = GetTransition(id);
            if (transition == null) { Debug.LogWarning("No such state found"); return; }

            ParentFsm.SetState(transition.Target);
        }
        #endregion
    }
}