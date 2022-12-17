using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerStateMachine : MonoBehaviour
{
    Rigidbody2D _rb;
    Animator _animator;

    [SerializeField] LayerMask layerMask;
    Vector2 _movementInput;
    bool _isDashing = false;

    [SerializeField] PlayerStats _stats;

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
        _rb.velocity = _movementInput * _stats.moveSpeed;

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
            Vector2 dir = _movementInput.normalized;

            if (!CanDash(dir, _stats.dashDistance)) return;

            _isDashing = true;

            transform.position += new Vector3(dir.x, dir.y, 0) * _stats.dashDistance;

            StartCoroutine(ResetDash());
        }
    }

    bool CanDash(Vector3 dir, float distance)
    {
        return Physics2D.Raycast(GetComponent<Collider2D>().bounds.center, dir, distance, ~layerMask).collider == null;
    }

    IEnumerator ResetDash()
    {
        yield return new WaitForSeconds(1f);
        _isDashing = false;
    }
}
