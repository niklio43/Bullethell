using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerStateMachine : MonoBehaviour
{
    Rigidbody2D _rb;

    [SerializeField] LayerMask layerMask;
    Vector2 _movementInput;
    bool _isDashing = false;

    [HideInInspector] public Weapon Weapon;

    //State variables
    PlayerBaseState _currentState;
    PlayerStateFactory _states;
    PlayerController _controller;

    //getters and setters
    public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    public Vector2 MovementInput { get { return _movementInput; } set { _movementInput = value; } }
    public bool IsDashing { get { return _isDashing; } set { _isDashing = value; } }

    float _ghostFadeTimer = 1;
    List<GameObject> _ghosts = new List<GameObject>();

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _controller = GetComponent<PlayerController>();

        //Setup state
        _states = new PlayerStateFactory(this);
        _currentState = _states.Idle();
        _currentState.EnterState();
    }

    void Update()
    {
        _currentState?.UpdateState();

        if (!IsDashing) return;
        _ghostFadeTimer -= Time.deltaTime;
        foreach(GameObject obj in _ghosts)
        {
            Color tmp = obj.GetComponent<SpriteRenderer>().color;
            tmp.a = _ghostFadeTimer;
            obj.GetComponent<SpriteRenderer>().color = tmp;
        }
    }

    void FixedUpdate()
    {
        _rb.velocity = _movementInput;

        if(_movementInput != Vector2.zero)
        {
            GetComponent<Animator>().Play("Walking");
        }
        else if(_movementInput == Vector2.zero)
        {
            GetComponent<Animator>().Play("Idle");
        }
    }

    public void Attack(int abilityIndex, InputAction.CallbackContext ctx)
    {
        if (Weapon == null) { Debug.Log("No Weapon Error"); return; }
        if (abilityIndex > Weapon.AbilitySlot.Count - 1 || Weapon.AbilitySlot[abilityIndex] == null) { Debug.Log("Ability doesn't exist!"); return; }

        Weapon.AbilitySlot[abilityIndex].Cast();
    }

    public void Move(InputAction.CallbackContext context)
    {
        _movementInput = context.ReadValue<Vector2>() * _controller.Stats["MoveSpeed"].Value;
    }

    #region Dash

    public void Dash(InputAction.CallbackContext context)
    {
        if (context.performed && !_isDashing)
        {
            Vector2 dir = _movementInput.normalized;

            if (!CanDash(dir, GetComponent<PlayerController>().Stats["DashDistance"].Value)) return;

            _isDashing = true;

            Vector3 targetPos = new Vector3(dir.x, dir.y, 0) * GetComponent<PlayerController>().Stats["DashDistance"].Value;

            transform.position += targetPos;

            InstantiateGhostTrail(dir);

            StartCoroutine(ResetDash());
        }
    }

    bool CanDash(Vector3 dir, float distance)
    {
        return Physics2D.Raycast(GetComponent<Collider2D>().bounds.center, dir, distance, ~layerMask).collider == null;
    }

    void InstantiateGhostTrail(Vector2 dir)
    {
        for (int i = 0; i < GetComponent<PlayerController>().Stats["DashDistance"].Value; i++)
        {
            GameObject ghost = new GameObject("Ghost");
            ghost.AddComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite;
            ghost.GetComponent<SpriteRenderer>().sortingOrder = 11;
            ghost.GetComponent<SpriteRenderer>().color = Color.grey;

            _ghosts.Add(ghost);

            ghost.transform.position = new Vector3((transform.position.x - (dir.x * i)), (transform.position.y - (dir.y * i)), 0);
            ghost.transform.rotation = transform.rotation;
            ghost.transform.localScale = transform.localScale;
        }
    }

    IEnumerator ResetDash()
    {
        yield return new WaitForSeconds(1f);
        _isDashing = false;
        _ghostFadeTimer = 1;
        foreach(GameObject obj in _ghosts)
        {
            Destroy(obj);
        }
        _ghosts.Clear();
    }

    #endregion
}
