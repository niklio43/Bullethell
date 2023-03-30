using UnityEngine;
using UnityEngine.InputSystem;
using BulletHell.Emitters;
using BulletHell.Player;
using BulletHell.UI;

public class PlayerControls : MonoBehaviour
{
    #region Private Fields
    [SerializeField] PlayerUI _playerUI;
    PlayerController _player;
    PlayerInputs _inputs;
    PlayerInteracter playerInteracter;
    #endregion

    #region Private Methods
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
        _inputs.Player.AbilityQ.performed += ctx => _player.WeaponController.UseAbility(0);
        _inputs.Player.AbilityE.performed += ctx => _player.WeaponController.UseAbility(1);
        _inputs.Player.AbilityR.performed += ctx => _player.WeaponController.UseAbility(2);

        //Move
        _inputs.Player.Move.performed += ctx => _player.PlayerMovement.Move(ctx);
        _inputs.Player.Move.canceled += ctx => _player.PlayerMovement.Move(ctx);

        //Look
        _inputs.Player.Look.performed += ctx => _player.PlayerMovement.Look(ctx);

        //UI
        _inputs.Player.Inventory.performed += ctx => _playerUI.ToggleInventory();
        _inputs.Player.Interact.performed += ctx => playerInteracter.Interact();
        _inputs.Player.OpenMap.performed += ctx => PlayerUI.Instance.ToggleLargeMap();
        _inputs.Player.OpenMap.canceled += ctx => PlayerUI.Instance.ToggleLargeMap();

        //Weapon
        _inputs.Player.Fire.performed += ctx => _player.WeaponController.Attack();
        _inputs.Player.EquipPrimaryWeapon.performed += ctx => _player.WeaponController.SetActiveWeapon(true);
        _inputs.Player.EquipSecondaryWeapon.performed += ctx => _player.WeaponController.SetActiveWeapon(false);
        #endregion
    }

    private void OnEnable() => _inputs?.Player.Enable();
    private void OnDisable() => _inputs?.Player.Disable();
    #endregion
}