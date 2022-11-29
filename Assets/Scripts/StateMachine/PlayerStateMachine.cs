using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerStateMachine : MonoBehaviour
{
    Rigidbody2D _rb;
    Animator _animator;

    //Movement variables
    int _speedMultiplier = 10;
    int _dashPower = 3000;
    Vector2 _movementInput;
    bool _isDashing = false;

    //State variables
    PlayerBaseState _currentState;
    PlayerStateFactory _states;

    //getters and setters
    public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public Vector2 MovementInput { get { return _movementInput; } set { _movementInput = value; } }
    public bool IsDashing { get { return _isDashing; } set { _isDashing = value; } }

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

        //Setup state
        _states = new PlayerStateFactory(this);
        _currentState = _states.Idle();
        _currentState.EnterState();
    }

    void Update()
    {
        _currentState?.UpdateState();
    }

    void FixedUpdate()
    {
        _rb.velocity = _movementInput * _speedMultiplier;

        HandleAnimation();

        if (_movementInput.x == 0) return;
        HandleRotation();
    }

    void HandleRotation()
    {
        if (_movementInput.x > 0) { transform.localRotation = Quaternion.Euler(0, 180, 0); }
        else { transform.localRotation = Quaternion.Euler(0, 0, 0); }
    }

    void HandleAnimation()
    {
        _animator.SetFloat("Horizontal", _movementInput.x);
        _animator.SetFloat("Vertical", _movementInput.y);
        _animator.SetFloat("Speed", _movementInput.sqrMagnitude);
    }

    public void Move(InputAction.CallbackContext context)
    {
        _movementInput = context.ReadValue<Vector2>();
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (context.performed && !_isDashing)
        {
            _isDashing = true;

            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector2 dir = mousePosition - transform.position;

            _rb.AddForce(dir.normalized * _dashPower, ForceMode2D.Force);

            StartCoroutine(ResetDash());
        }
    }

    IEnumerator ResetDash()
    {
        yield return new WaitForSeconds(1f);
        _isDashing = false;
    }
}
