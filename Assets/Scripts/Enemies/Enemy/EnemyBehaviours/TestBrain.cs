using BulletHell.Enemies.Detection;
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
           .State(EnemyStates.Idle, (foundPlayer) => {
               foundPlayer.SetTransition("foundPlayer", EnemyStates.Chasing)
               .Update((action) => {
                   if (enemy.Target != null) {
                       action.Transition("foundPlayer");
                   }
               });
           })
           .State(EnemyStates.Chasing, (chasingPlayer) => {
               chasingPlayer.SetTransition("attackPlayer", EnemyStates.Attacking)
               .Update((action) => {
               });
           })
           .Build();
        }
    }
}
