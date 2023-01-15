using UnityEngine;
using UnityEngine.InputSystem;
using BulletHell.Emitters;
using BulletHell.Player;

public class PlayerControls : MonoBehaviour
{
    [SerializeField] PlayerUI _playerUI;
    Player _player;
    PlayerInputs _inputs;
    PlayerInteracter playerInteracter;
    void Awake()
    {
        playerInteracter = GetComponent<PlayerInteracter>();
        _player = GetComponent<Player>();

        _inputs = new PlayerInputs();

        #region Input bindings

        //Dash
        _inputs.Player.Dash.performed += ctx => _player.Dash(ctx);

        //Ability
        _inputs.Player.AbilityQ.performed += ctx => _player.Attack(1, ctx);
        _inputs.Player.AbilityE.performed += ctx => _player.Attack(2, ctx);
        _inputs.Player.AbilityR.performed += ctx => _player.Attack(3, ctx);

        //Move
        _inputs.Player.Move.performed += ctx => _player.Move(ctx);
        _inputs.Player.Move.canceled += ctx => _player.Move(ctx);

        //UI
        _inputs.Player.Inventory.performed += ctx => _playerUI.ToggleInventory();
        _inputs.Player.Interact.performed += ctx => playerInteracter.Interact();

        //Fire
        _inputs.Player.Fire.performed += ctx => _player.Attack(0, ctx);
        #endregion
    }

    private void OnEnable() => _inputs?.Player.Enable();
    private void OnDisable() => _inputs?.Player.Disable();
}