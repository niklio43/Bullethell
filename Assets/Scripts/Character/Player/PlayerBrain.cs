using BulletHell.FiniteStateMachine;
using UnityEngine;

namespace BulletHell.Player
{
    public class PlayerBrain
    {
        public IFSM _FSM;
        PlayerController _player;

        public enum PlayerStates
        {
            Default,
            Interacting,
            Dashing,
            Staggered
        }

        public PlayerBrain(PlayerController player)
        {
            _player = player;
            Initialize();
        }

        public void UpdateBrain()
        {
            _FSM.Update();
        }

        void Initialize()
        {
            _FSM = new FSMBuilder()
           .Owner(_player.gameObject)
           .Default(PlayerStates.Default)
           .State(PlayerStates.Default, (Default) =>
           {
               Default.SetTransition("interacting", PlayerStates.Interacting);
               Default.SetTransition("dashing", PlayerStates.Dashing)
               .Update((action) =>
               {
                   _rb.velocity = _movementInput;
                   if (_player.IsDashing)
                   {
                       action.Transition("dashing");
                   }
                   if (_player.IsInteracting)
                   {
                       action.Transition("interacting");
                   }
               });
           })
           .State(PlayerStates.Interacting, (interacting) =>
           {
               interacting.SetTransition("default", PlayerStates.Default)
               .Update((action) =>
               {
                   if (_player.IsInteracting) return;
                   action.Transition("default");
               });
           })
           .State(PlayerStates.Dashing, (dashing) =>
           {
               dashing.SetTransition("default", PlayerStates.Default)
               .Enter((action) =>
               {
                   _player.IsInvincible = true;
               })
               .Update((action) =>
               {
                   if (_player.IsDashing) return;
                   action.Transition("default");
               })
               .Exit((action) =>
                {
                    _player.IsInvincible = false;
                });
           })
           .State(PlayerStates.Staggered, (staggered) =>
           {
               staggered.SetTransition("default", PlayerStates.Default)
               .Enter((action) =>
               {
                   _player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
               })
               .Exit((action) =>
               {
                   _player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                   _player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
               });
           })
           .Build();
        }
    }
}
