using BulletHell.Emitters;
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
               foundPlayer.SetTransition("chasePlayer", EnemyStates.Chasing)
               .Update((action) => {
                   if (enemy.Target != null) {
                       action.Transition("chasePlayer");
                   }
               });
           })
           .State(EnemyStates.Chasing, (chasingPlayer) => {
               chasingPlayer.SetTransition("attackPlayer", EnemyStates.Attacking)
               .Update((action) => {
                   enemy.GetComponent<Animator>().SetBool("Running", (enemy.GetComponent<Rigidbody2D>().velocity.SqrMagnitude() > 1));
                   if (enemy.Stats.AttackTimer < 0 && Vector2.Distance(enemy.transform.position, enemy.Target.position) < enemy.Stats.AttackDistance) {
                       action.Transition("attackPlayer");
                   }
               });
           })
           .State(EnemyStates.Attacking, (attackingPlayer) => {
               attackingPlayer.SetTransition("chasePlayer", EnemyStates.Chasing)
               .SetAnimationClip("Idle")
               .Update((action) => {
                   enemy.GetComponentInChildren<Emitter>().SetDirection((enemy.Target.position - enemy.transform.position).normalized);
                   enemy.GetComponentInChildren<Emitter>().FireProjectile();
                   enemy.Stats.AttackTimer = 1;
                   action.Transition("chasePlayer");
               });
           })
           .Build();
        }
    }
}
