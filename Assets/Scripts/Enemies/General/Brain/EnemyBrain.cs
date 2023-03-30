using BulletHell.Abilities;
using BulletHell.FiniteStateMachine;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.Enemies
{
    [CreateAssetMenu(fileName = "EnemyBrain", menuName = "Enemies/Brains/Default Brain", order = 1)]
    public class EnemyBrain : ScriptableObject
    {
        #region Private Fields
        [SerializeField] protected List<EnemyAbility> abilities = new List<EnemyAbility>();
        Enemy _owner;
        bool _canAttack = true;

        float attackCoolDown = 1.5f;
        float _currentAttackCoolDown = 0;
        #endregion

        #region Public Fields
        [HideInInspector] public EnemyAbility CurrentAbility = null;
        public IFSM FSM { get; protected set; }

        [HideInInspector] public bool LockState = false;

        public enum EnemyStates
        {
            Idle,
            Chasing,
            Attacking,
            Staggered,
            Stunned
        }
        #endregion

        #region Public Methods
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

            state.Enter((action) => {
                _canAttack = true;
            });
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

                if (CanCastAbility() && _canAttack && _currentAttackCoolDown > attackCoolDown) {
                    action.Transition("attack");
                }
            });

            return state;
        }

        public virtual StateBuilder AttackState(StateBuilder state)
        {
            state.SetTransition("idle", EnemyStates.Idle);
            state.Enter((action) => {
                _currentAttackCoolDown = 0;
                _owner.CanMove = false;
                _canAttack = false;
                CurrentAbility = null;

                foreach (EnemyAbility ability in abilities) {
                    if (ability.CanCast()) {
                        CurrentAbility = ability;
                    }
                }

                if (CurrentAbility == null) { action.Transition("idle"); return; }
                CurrentAbility.Cast(_owner.Target.position, () => { action.Transition("idle"); });
            });

            return state;
        }

        public virtual StateBuilder StaggeredState(StateBuilder state)
        {
            state.SetAnimationClip("Stagger");
            state.Enter((action) => {
                _owner.CanMove = false;
                _canAttack = false;

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
                _canAttack = false;
            });
            state.Exit((action) => {
                _owner.GetComponent<Animator>().speed = 1;
            });
            return state;
        }

        public virtual void UpdateBrain(float dt)
        {
            FSM.Update();

            _currentAttackCoolDown += dt;

            foreach (EnemyAbility ability in abilities) {
                ability.UpdateAbility(Time.deltaTime);
            }
        }

        public void SetState(EnemyStates state)
        {
            if(LockState) { return; }
            FSM.SetState(state);
        }
        #endregion

        #region Private Methods
        bool CanCastAbility()
        {
            foreach (EnemyAbility ability in abilities) {
                if (ability.CanCast()) {
                    return true;
                }
            }

            return false;
        }

        void InitializeAbilities()
        {
            for (int i = 0; i < abilities.Count; i++) {
                abilities[i].Initialize(_owner);
            }
        }
        #endregion

    }
}