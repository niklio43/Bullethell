using BulletHell.FiniteStateMachine;
using UnityEngine;

namespace BulletHell.Player
{
    public class PlayerBrain : MonoBehaviour
    {
        IFSM _FSM;
        Player _player;

        public enum PlayerStates
        {
            Default,
            Interacting,
            Dashing
        }

        void Awake()
        {
            _player = GetComponent<Player>();

            _FSM = new FSMBuilder()
           .Owner(gameObject)
           .Default(PlayerStates.Default)
           .State(PlayerStates.Default, (Default) => {
               Default.SetTransition("interacting", PlayerStates.Interacting);
               Default.SetTransition("dashing", PlayerStates.Dashing)
               .Update((action) => {
                   //conditiong for transition
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
           .State(PlayerStates.Interacting, (interacting) => {
               interacting.SetTransition("default", PlayerStates.Default)
               .Update((action) => {
                   //conditiong for transition
                   if (_player.IsInteracting) return;
                   action.Transition("default");
               });
           })
           .State(PlayerStates.Dashing, (dashing) => {
               dashing.SetTransition("default", PlayerStates.Default)
               .Update((action) => {
                   if (_player.IsDashing) return;
                   action.Transition("default");
               });
           })
           .Build();
        }
    }
}
