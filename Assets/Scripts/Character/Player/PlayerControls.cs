using UnityEngine;
using UnityEngine.InputSystem;
using BulletHell.Emitters;
using BulletHell.Player;
using BulletHell.UI;

public class PlayerControls : MonoBehaviour
{
    [SerializeField] PlayerUI _playerUI;
    [SerializeField] WeaponController _weaponController;
    PlayerController _player;
    PlayerInputs _inputs;
    PlayerInteracter playerInteracter;
    void Awake()
    {
        playerInteracter = GetComponent<PlayerInteracter>();
        _player = GetComponent<PlayerController>();

        _inputs = new PlayerInputs();

        #region Input bindings

        //Dash
        _inputs.Player.Dash.performed += ctx => _player.PlayerAbilities.Dash(0, ctx);
        _inputs.Player.Parry.performed += ctx => _player.PlayerAbilities.Parry(1, ctx);

        //Ability
        _inputs.Player.AbilityQ.performed += ctx => _weaponController.Attack(1, ctx);
        _inputs.Player.AbilityE.performed += ctx => _weaponController.Attack(2, ctx);
        _inputs.Player.AbilityR.performed += ctx => _weaponController.Attack(3, ctx);

        //Move
        _inputs.Player.Move.performed += ctx => _player.PlayerMovement.Move(ctx);
        _inputs.Player.Move.canceled += ctx => _player.PlayerMovement.Move(ctx);

        //UI
        _inputs.Player.Inventory.performed += ctx => _playerUI.ToggleInventory();
        _inputs.Player.Interact.performed += ctx => playerInteracter.Interact();

        //Fire
        _inputs.Player.Fire.performed += ctx => _weaponController.Attack(0, ctx);
        #endregion
    }

    private void OnEnable() => _inputs?.Player.Enable();
    private void OnDisable() => _inputs?.Player.Disable();
}