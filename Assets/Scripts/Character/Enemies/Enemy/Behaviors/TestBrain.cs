using BulletHell.FiniteStateMachine;
using UnityEngine;

namespace BulletHell.Enemies
{
    [CreateAssetMenu(fileName = "TestBrain", menuName = "Enemies/Brains/TestBrain", order = 1)]
    public class TestBrain : EnemyBrain
    {
        public override void Initialize(Enemy enemy)
        {
            FSM = new FSMBuilder()
           .Owner(enemy.gameObject)
           .Default(EnemyStates.Idle)
           .State(EnemyStates.Idle, (idle) => {
               idle.SetTransition("chasePlayer", EnemyStates.Chasing)
               .Update((action) => {
                   if (enemy.Target != null) {
                       action.Transition("chasePlayer");
                   }
               });
           })
           .State(EnemyStates.Chasing, (chasing) => {
               chasing.SetTransition("attackPlayer", EnemyStates.Attacking)
               .Update((action) => {
                   enemy.CanMove = true;

                   if (enemy.GetComponent<Rigidbody2D>().velocity.magnitude > .1f) {
                       enemy.Animator.Play("Walk");
                   }
                   else {
                       enemy.Animator.Play("Idle");
                   }

                   if (enemy.TargetInAttackRange() && enemy.Abilities.CanAttack()) {
                       action.Transition("attackPlayer");
                   }
               });
           })
           .State(EnemyStates.Attacking, (attacking) => {
               attacking.SetTransition("idle", EnemyStates.Idle)
               .SetAnimationClip("Attack")
               .Update((action) => {
                   enemy.CanMove = false;
                   enemy.Animator.Play("Attack");

                   MonoInstance.Instance.Invoke(() => { 
                       enemy.Abilities.CastOrderedAbility();
                       action.Transition("idle");
                   }, enemy.Animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);
               });
           })
           .Build();
        }
    }
}
