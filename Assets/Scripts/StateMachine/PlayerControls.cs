using UnityEngine;
using UnityEngine.InputSystem;
using BulletHell.Emitters;

public class PlayerControls : MonoBehaviour
{
    [SerializeField] PlayerStateMachine _playerStateMachine;
    [SerializeField] PlayerUI _playerUI;
    [SerializeField] Emitter _emitter;
    PlayerInputs _inputs;

    void Awake()
    {
        _inputs = new PlayerInputs();

        #region Input bindings

        //Dash
        _inputs.Player.Dash.performed += ctx => _playerStateMachine.Dash(ctx);

        //Move
        _inputs.Player.Move.performed += ctx => _playerStateMachine.Move(ctx);
        _inputs.Player.Move.canceled += ctx => _playerStateMachine.Move(ctx);

        //Hotbar
        _inputs.Player.LeftHotBar.performed += ctx => _playerUI.LeftHotBar(ctx);
        _inputs.Player.RightHotBar.performed += ctx => _playerUI.RightHotBar(ctx);
        _inputs.Player.BottomHotBar.performed += ctx => _playerUI.BottomHotBar(ctx);
        _inputs.Player.TopHotBar.performed += ctx => _playerUI.TopHotBar(ctx);

        //Fire projectile
        _inputs.Player.Fire.started += ctx => _emitter.FireProjectile();

        #endregion
    }

    private void OnEnable() => _inputs?.Player.Enable();
    private void OnDisable() => _inputs?.Player.Disable();
}