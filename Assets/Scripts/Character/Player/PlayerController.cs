using System;
using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using BulletHell.Abilities;
using BulletHell.Stats;
using Bullet.CameraUtilities;

namespace BulletHell.Player
{
    public class PlayerController : Character
    {
        Rigidbody2D _rb;
        Vector2 _movementInput;
        bool _isDashing = false;
        bool _isInteracting = false;

        [SerializeField] List<Ability> _abilities = new List<Ability>();

        [SerializeField] PlayerAfterImageSprite _afterImage;
        [SerializeField] float _amountOfImages = 4;
        [SerializeField] float _dashTime = .1f;
        public PlayerAimWeapon Aim;

        ObjectPool<PlayerAfterImageSprite> _afterImagePool;

        public ObjectPool<PlayerAfterImageSprite> AfterImagePool { get { return _afterImagePool; } }
        public float AmountOfImages { get { return _amountOfImages; } }
        public float DashTime { get { return _dashTime; } }
        public Rigidbody2D Rb { get { return _rb; } }
        public bool IsDashing { get { return _isDashing; } set { _isDashing = value; } }
        public bool IsInteracting { get { return _isInteracting; } set { _isInteracting = value; } }
        public Vector2 MovementInput { get { return _movementInput; } set { _movementInput = value; } }

        void Awake()
        {
            Initialize();

            _afterImagePool = new ObjectPool<PlayerAfterImageSprite>(CreateAfterImage, (int)_amountOfImages, "AfterImagePool");

            for (int i = 0; i < _abilities.Count; i++)
            {
                _abilities[i] = Instantiate(_abilities[i]);
                _abilities[i].Initialize(gameObject);
            }

            _rb = GetComponent<Rigidbody2D>();
        }

        protected override void OnUpdate()
        {
            foreach (Ability ability in _abilities)
            {
                ability.UpdateAbility(Time.deltaTime);
            }
        }

        void FixedUpdate()
        {
            if (!_isDashing)
            {
                _rb.velocity = _movementInput;
            }

            if (_movementInput == Vector2.zero) { _animator.Play("Idle"); return; }
            _animator.Play("Walking");
        }

        public void Move(InputAction.CallbackContext context)
        {
            _movementInput = context.ReadValue<Vector2>() * Stats["MoveSpeed"].Value;
        }

        public void Dash(int abilityIndex, InputAction.CallbackContext context)
        {
            if (context.performed && !_isDashing)
            {
                _abilities[abilityIndex].Cast();
            }
        }

        PlayerAfterImageSprite CreateAfterImage()
        {
            PlayerAfterImageSprite afterImage = Instantiate(_afterImage);

            afterImage.Pool = _afterImagePool;

            return afterImage;
        }

        public override void TakeDamage(float damage)
        {
            base.TakeDamage(damage);

            Camera.main.Shake(0.1f, 0.2f);

        }

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