using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BulletHell.FiniteStateMachine
{
    [Serializable]
    public class FSM : IFSM
    {
        #region Private Fields
        private readonly Dictionary<Enum, IState> _states = new Dictionary<Enum, IState>();
        #endregion

        #region Public Fields
        public GameObject Owner { get; }

        public IState CurrentState { get; private set; }

        public IState DefaultState { get; set; }

        public UnityEvent EventExit { get; }

        public FSM (GameObject owner)
        {
            Owner = owner;
        }
        #endregion

        #region Public Methods
        public IState GetState(Enum id)
        {
            return _states[id];
        }

        public void AddState(IState state)
        {
            _states[state.Id] = state;
        }

        public void SetState(Enum id)
        {
            CurrentState?.Exit();
            CurrentState = GetState(id);
            CurrentState?.Enter();
        }
        public void Update()
        {
            if(CurrentState == null && DefaultState != null) {
                SetState(DefaultState.Id);
            }

            CurrentState?.Update();
        }
        public void Reset()
        {
            SetState(DefaultState.Id);
        }

        public void Exit()
        {
            CurrentState.Exit();
            CurrentState = null;
            EventExit.Invoke();
        }
        #endregion
    }
}