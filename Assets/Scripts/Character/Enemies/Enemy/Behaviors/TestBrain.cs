using BulletHell.Abilities;
using BulletHell.FiniteStateMachine;
using UnityEngine;

namespace BulletHell.Enemies
{
    [CreateAssetMenu(fileName = "TestBrain", menuName = "Enemies/Brains/TestBrain", order = 1)]
    public class TestBrain : EnemyBrain
    {
        [SerializeField] Ability ability_1, ability_2;

        public enum EnemyStates
        {
            Idle,
            Chasing,
            Attack_1,
            Attack_2
        }
        public override void Initialize(Enemy enemy)
        {
            ability_1.Initialize(enemy.gameObject, enemy.Aim.gameObject);
            ability_2.Initialize(enemy.gameObject, enemy.Aim.gameObject);

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
               chasing.SetTransition("attack_1", EnemyStates.Attack_1);
               chasing.SetTransition("attack_2", EnemyStates.Attack_2)
               .Update((action) => {
                   enemy.CanMove = true;

                   if (enemy.GetComponent<Rigidbody2D>().velocity.magnitude > .1f) {
                       enemy.Animator.Play("Walk");
                   }
                   else {
                       enemy.Animator.Play("Idle");
                   }

                   if (enemy.TargetInAttackRange()) {
                       if (ability_1.CanCast())
                           action.Transition("attack_1");
                       else if (ability_2.CanCast())
                         action.Transition("attack_2");
                   }
               });
           })
           .State(EnemyStates.Attack_1, (attack_1) => {
               attack_1.SetTransition("idle", EnemyStates.Idle)
               .Enter((action) => {
                   enemy.CanMove = false;
                   enemy.Animator.Play("Attack");
                   enemy.Animator.Update(Time.deltaTime);
                   float waitTime = enemy.Animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;

                   MonoInstance.Instance.Invoke(() => {
                       ability_1.Cast();
                       action.Transition("idle");
                   }, waitTime);
               });
           })
          .State(EnemyStates.Attack_2, (attack_2) => {
              attack_2.SetTransition("idle", EnemyStates.Idle)
              .Enter((action) => {
                  enemy.CanMove = false;
                  enemy.Animator.Play("Attack2");
                  enemy.Animator.Update(Time.deltaTime);
                  float waitTime = enemy.Animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;

                  MonoInstance.Instance.Invoke(() => {
                      ability_2.Cast();
                      action.Transition("idle");
                  }, waitTime);
              });
          })
           .Build();
        }

        public override void Think()
        {
            base.Think();
            ability_2.UpdateAbility(Time.deltaTime);
            ability_1.UpdateAbility(Time.deltaTime);
        }
    }
}
