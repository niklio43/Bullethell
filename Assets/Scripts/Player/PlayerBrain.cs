using BulletHell.FiniteStateMachine;
using UnityEngine;

namespace BulletHell.Player
{
    public class PlayerBrain
    {
        #region Public Fields
        public IFSM _FSM;
        public enum PlayerStates
        {
            Default,
            Interacting,
            Dashing,
            Staggered
        }
        #endregion

        #region Private Fields
        PlayerController _player;
        #endregion

        #region Public Methods
        public PlayerBrain(PlayerController player)
        {
            _player = player;
            Initialize();
        }

        public void UpdateBrain()
        {
            _FSM.Update();
        }
        #endregion

        #region Private Methods
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
                   if (_player.PlayerAbilities.IsDashing)
                   {
                       action.Transition("dashing");
                       return;
                   }
                   if (_player.PlayerAbilities.IsInteracting)
                   {
                       action.Transition("interacting");
                       return;
                   }

                   _player.PlayerMovement.Rb.velocity = _player.PlayerMovement.MovementInput;
               });
           })
           .State(PlayerStates.Interacting, (interacting) =>
           {
               interacting.SetTransition("default", PlayerStates.Default)
               .Update((action) =>
               {
                   if (_player.PlayerAbilities.IsInteracting) return;
                   action.Transition("default");
               });
           })
           .State(PlayerStates.Dashing, (dashing) =>
           {
               dashing.SetTransition("default", PlayerStates.Default)
               .Enter((action) =>
               {
                   _player.PlayerAbilities.IsInvincible = true;
               })
               .Update((action) =>
               {
                   if (_player.PlayerAbilities.IsDashing) return;
                   action.Transition("default");
               })
               .Exit((action) =>
                {
                    _player.PlayerAbilities.IsInvincible = false;
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
        #endregion
    }
}
