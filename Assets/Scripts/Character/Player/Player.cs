using System;
using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using System.Collections.Generic;

namespace BulletHell.Player
{
    public class Player : Character
    {
        Rigidbody2D _rb;
        Vector2 _movementInput;
        float _ghostFadeTimer = 1;
        public bool _isDashing = false;
        List<GameObject> _ghosts = new List<GameObject>();
        [HideInInspector] public Weapon Weapon;

        [SerializeField] LayerMask layerMask;

        public bool IsDashing { get { return _isDashing; } set { _isDashing = value; } }
        public Vector2 MovementInput { get { return _movementInput; } set { _movementInput = value; } }

        void Awake()
        {
            Initialize();

            _rb = GetComponent<Rigidbody2D>();
        }

        void FixedUpdate()
        {
            _rb.velocity = _movementInput;

            if (_movementInput == Vector2.zero) { _animator.Play("Idle"); return; }
            _animator.Play("Walking");
        }

        void Update()
        {
            if (!_isDashing) return;
            _ghostFadeTimer -= Time.deltaTime;
            foreach (GameObject obj in _ghosts)
            {
                Color tmp = obj.GetComponent<SpriteRenderer>().color;
                tmp.a = _ghostFadeTimer;
                obj.GetComponent<SpriteRenderer>().color = tmp;
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
            _movementInput = context.ReadValue<Vector2>() * Stats["MoveSpeed"].Value;
        }

        #region Dash

        public void Dash(InputAction.CallbackContext context)
        {
            if (context.performed && !_isDashing)
            {
                Vector2 dir = _movementInput.normalized;

                if (!CanDash(dir, Stats["DashDistance"].Value)) return;

                _isDashing = true;

                Vector3 targetPos = new Vector3(dir.x, dir.y, 0) * Stats["DashDistance"].Value;

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
            for (int i = 0; i < Stats["DashDistance"].Value; i++)
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
            foreach (GameObject obj in _ghosts)
            {
                Destroy(obj);
            }
            _ghosts.Clear();
        }

        #endregion

        #region Component Caching

        Dictionary<Type, Component> _cachedComponents = new Dictionary<Type, Component>();
        public new T GetComponent<T>() where T : Component
        {
            if (_cachedComponents.ContainsKey(typeof(T)))
            {
                return (T)_cachedComponents[typeof(T)];
            }

            var component = base.GetComponent<T>();
            if (component != null)
            {
                _cachedComponents.Add(typeof(T), component);
            }
            return component;
        }

        #endregion
    }
}