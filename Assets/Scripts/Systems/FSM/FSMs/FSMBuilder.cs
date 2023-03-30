using System;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.FiniteStateMachine
{
    public class FSMBuilder
    {
        private class StateData
        {
            public Enum id;
            public Action<StateBuilder> callback;
        }

        #region Private Fields
        private readonly List<StateData> _stateData = new List<StateData>();
        private GameObject _owner;
        private Enum _defaultState;
        #endregion

        #region Public Methods
        public FSMBuilder Owner(GameObject owner)
        {
            _owner = owner;
            return this;
        }

        public FSMBuilder State(Enum id, Action<StateBuilder> stateCallback)
        {
            var builder = new StateBuilder { Id = id };
            stateCallback(builder);
            _stateData.Add(new StateData
            {
                id = id,
                callback = stateCallback
            });

            return this;
        }

        public FSMBuilder Default(Enum id)
        {
            _defaultState = id;
            return this;
        }

        public IFSM Build()
        {
            var fsm = new FSM(_owner) as IFSM;
            StateData defaultState = null;

            foreach (var state in _stateData) {
                var builder = new StateBuilder { Id = state.id };
                state.callback(builder);
                fsm.AddState(builder.Build(fsm));

                if (Equals(_defaultState, state.id)) {
                    defaultState = state;
                }
            }
            SetupDefaultState(defaultState, fsm);

            return fsm;
        }
        #endregion

        #region Private Methods
        private void SetupDefaultState(StateData defaultState, IFSM fsm)
        {
            if (_stateData.Count == 0) return;

            if (defaultState == null) {
                defaultState = _stateData[0];
            }

            fsm.DefaultState = fsm.GetState(defaultState.id);
            fsm.SetState(fsm.DefaultState.Id);
        }
        #endregion
    }
}