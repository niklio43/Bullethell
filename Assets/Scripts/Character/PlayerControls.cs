using UnityEngine;
using UnityEngine.InputSystem;
using BulletHell.Emitters;

public class PlayerControls : MonoBehaviour
{
    [SerializeField] PlayerStateMachine _playerStateMachine;
    [SerializeField] PlayerUI _playerUI;
    PlayerInputs _inputs;
    PlayerInteracter playerInteracter;
    void Awake()
    {
        playerInteracter = GetComponent<PlayerInteracter>();

        _inputs = new PlayerInputs();

        #region Input bindings

        //Dash
        _inputs.Player.Dash.performed += ctx => _playerStateMachine.Dash(ctx);

        //Ability
        _inputs.Player.AbilityQ.performed += ctx => _playerStateMachine.Attack(1, ctx);
        _inputs.Player.AbilityE.performed += ctx => _playerStateMachine.Attack(2, ctx);
        _inputs.Player.AbilityR.performed += ctx => _playerStateMachine.Attack(3, ctx);

        //Move
        _inputs.Player.Move.performed += ctx => _playerStateMachine.Move(ctx);
        _inputs.Player.Move.canceled += ctx => _playerStateMachine.Move(ctx);

        //UI
        _inputs.Player.Inventory.performed += ctx => _playerUI.ToggleInventory();
        _inputs.Player.Interact.performed += ctx => playerInteracter.Interact();

        //Fire
        _inputs.Player.Fire.performed += ctx => _playerStateMachine.Attack(0, ctx);
        #endregion
    }

    private void OnEnable() => _inputs?.Player.Enable();
    private void OnDisable() => _inputs?.Player.Disable();
}