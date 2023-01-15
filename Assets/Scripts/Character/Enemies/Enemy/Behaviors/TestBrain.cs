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
                   /*if (enemy.TargetInAttackRange() && enemy.GetComponent<EnemyAttack>().CanAttack()) {
                       action.Transition("attackPlayer");
                   }*/
               });
           })
           .State(EnemyStates.Attacking, (attacking) => {
               attacking.SetTransition("chasePlayer", EnemyStates.Chasing)
               .Update((action) => {
                   attacking.SetAnimatorBool("Attacking", true);
                   enemy.CanMove = false;
                   //enemy.GetComponent<EnemyAttack>().CastOrderedAbility(enemy.Target);




                   action.Transition("chasePlayer");
               });
           })
           .Build();
        }
    }
}