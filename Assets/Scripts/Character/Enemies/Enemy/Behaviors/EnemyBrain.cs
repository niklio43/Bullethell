using BulletHell.Abilities;
using BulletHell.FiniteStateMachine;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.Enemies
{
    [CreateAssetMenu(fileName = "EnemyBrain", menuName = "Enemies/Brains/Default Brain", order = 1)]
    public class EnemyBrain : ScriptableObject
    {
        [SerializeField] protected List<Ability> abilities = new List<Ability>();
        public IFSM FSM { get; protected set; }
        Enemy _owner;

        bool _canAttack = true;

        public enum EnemyStates
        {
            Idle,
            Chasing,
            Attacking,
            Staggered,
            Stunned
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
            .State(EnemyStates.Staggered, (stagger) => StaggeredState(stagger))
            .State(EnemyStates.Stunned, (stun) => StunnedState(stun))
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

                if (_owner.TargetInAttackRange() && CanCastAbility() && _canAttack) {
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
                _canAttack = false;
                Ability chosenAbility = null;

                foreach (Ability ability in abilities) {
                    if (ability.CanCast()) {
                        chosenAbility = ability;
                    }
                }

                if (chosenAbility == null) { action.Transition("idle"); return; }

                chosenAbility.Cast(() => action.Transition("idle"));
                _owner.Invoke(() => _canAttack = true, 1f);
            });

            return state;
        }

        public virtual StateBuilder StaggeredState(StateBuilder state)
        {
            state.SetAnimationClip("Stagger");
            state.Enter((action) => {
                _owner.CanMove = false;

                float waitTime = _owner.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length;
                _owner.Invoke(() => SetState(EnemyStates.Idle), waitTime);
            });
            return state;
        }

        public virtual StateBuilder StunnedState(StateBuilder state)
        {
            state.Enter((action) => {
                _owner.GetComponent<Animator>().speed = 0;
                _owner.CanMove = false;
            });
            state.Exit((action) => {
                _owner.GetComponent<Animator>().speed = 1;
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

        public void SetState(EnemyStates state)
        {
            FSM.SetState(state);
        }

    }
}