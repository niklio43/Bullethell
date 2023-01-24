using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.FiniteStateMachine;
using BulletHell.Enemies.Detection;
using BulletHell.Abilities;

namespace BulletHell.Enemies
{
    [CreateAssetMenu(fileName = "EnemyBrain", menuName = "Enemies/Brains/Default Brain", order = 1)]
    public class EnemyBrain : ScriptableObject
    {
        [SerializeField] protected List<Ability> abilities = new List<Ability>();
        public IFSM FSM { get; protected set; }
        Enemy _owner;

        public enum EnemyStates
        {
            Idle,
            Chasing,
            Attacking
        }

        public virtual void Initialize(Enemy owner)
        {
            _owner = owner;
            InitializeAbilities();

            FSM = new FSMBuilder()
            .Owner(owner.gameObject)
            .Default(EnemyStates.Idle)
            .State(EnemyStates.Idle, (idle) => IdleState(idle))
            .State(EnemyStates.Chasing, (chase) => ChaseState(chase))
            .State(EnemyStates.Attacking, (attack) => AttackState(attack))
            .Build();
        }

        public virtual StateBuilder IdleState(StateBuilder state)
        {
            state.SetAnimationClip("Idle");
            state.SetTransition("chasePlayer", EnemyStates.Chasing);
            state.Update((action) => {

                if (_owner.Target != null) {
                  action.Transition("chasePlayer");
              }
          });

            return state;
        }
        public virtual StateBuilder ChaseState(StateBuilder state)
        {
            state.SetTransition("attack", EnemyStates.Attacking);
            state.Update((action) => {
                _owner.CanMove = true;

                if (_owner.GetComponent<Rigidbody2D>().velocity.magnitude > .1f) {
                    _owner.GetComponent<Animator>().Play("Walk");
                }
                else {
                    _owner.GetComponent<Animator>().Play("Idle");
                }

                if (_owner.TargetInAttackRange() && CanCastAbility()) {
                    action.Transition("attack");
                }
            });

            return state;
        }

        public virtual StateBuilder AttackState(StateBuilder state)
        {
            state.SetTransition("idle", EnemyStates.Idle);
            state.Enter((action) => {
                _owner.CanMove = false;
                Ability chosenAbility = null;

                foreach (Ability ability in abilities) {
                    if (ability.CanCast()) {
                        chosenAbility = ability;
                    }
                }

                if(chosenAbility == null) { action.Transition("idle"); return; }

                chosenAbility.Cast(() => action.Transition("idle"));

            });

            return state;
        }


        bool CanCastAbility()
        {
            foreach (Ability ability in abilities) {
                if (ability.CanCast()) {
                    return true;
                }
            }

            return false;
        }

        void InitializeAbilities()
        {
            for (int i = 0; i < abilities.Count; i++) {
                abilities[i] = Instantiate(abilities[i]);

                GameObject host = (_owner.Weapon != null) ? _owner.Weapon.gameObject : _owner.Aim.gameObject;

                abilities[i].Initialize(_owner.gameObject, host);
            }
        }

        public virtual void Update()
        {
            FSM.Update();

            foreach (Ability ability in abilities) {
                ability.UpdateAbility(Time.deltaTime);
            }
        }
    }
}