using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.FiniteStateMachine
{
    public class StateBuilder
    {
        public Enum Id { get; set; }
        private readonly List<ITransition> _transitions = new List<ITransition>();
        private readonly List<IAction> _actions = new List<IAction>();

        public StateBuilder SetTransition(string change, Enum id)
        {
            _transitions.Add(new Transition(change, id));
            return this;
        }
        public StateBuilder SetAnimationClip(string clipName)
        {
            return AddAction(new ActionSetAnimationClip(clipName));
        }

        public StateBuilder SetAnimationClip(AnimationClip clip)
        {
            return AddAction(new ActionSetAnimationClip(clip));
        }
        public StateBuilder SetAnimatorTrigger(string name)
        {
            return AddAction(new ActionSetAnimatorTrigger(name));
        }

        public StateBuilder SetAnimatorBool(string name, bool value)
        {
            return AddAction(new ActionSetAnimatorBool(name, value));
        }

        public StateBuilder SetAnimatorInt(string name, int value)
        {
            return AddAction(new ActionSetAnimatorInt(name, value));
        }

        public StateBuilder SetAnimatorFloat(string name, float value)
        {
            return AddAction(new ActionSetAnimatorFloat(name, value));
        }

        public StateBuilder Update(Action<IAction> action)
        {
            return AddAction(new ActionUpdate(action));
        }

        public StateBuilder Update (string actionName, Action<IAction> action)
        {
            return AddAction(new ActionUpdate(action)
            {
                Name = actionName,
            });
        }

        public StateBuilder Enter(Action<IAction> action)
        {
            return AddAction(new ActionEnter(action));
        }

        public StateBuilder Enter(string actionName, Action<IAction> action)
        {
            return AddAction(new ActionEnter(action)
            {
                Name = actionName,
            });
        }

        public StateBuilder Exit(Action<IAction> action)
        {
            return AddAction(new ActionExit(action));
        }

        public StateBuilder Exit(string actionName, Action<IAction> action)
        {
            return AddAction(new ActionExit(action)
            {
                Name = actionName,
            });
        }

        public StateBuilder RunFSM(string exitTransition, IFSM fsm)
        {
            return AddAction(new ActionRunFSM(exitTransition, fsm));
        }

        public StateBuilder FSMExit()
        {
            return AddAction(new ActionExitFSM());
        }

        public StateBuilder AddAction(IAction action)
        {
            _actions.Add(action);
            return this;
        }

        public IState Build(IFSM fsm)
        {
            var state = new State(fsm, Id);

            foreach (var transition in _transitions) {
                state.AddTransition(transition);
            }

            foreach (var action in _actions) {
                action.ParentState = state;
                state.Actions.Add(action);
            }

            return state;
        }
    }
}
