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
    public class PlayerController : MonoBehaviour
    {
        Rigidbody2D _rb;
        Vector2 _movementInput;
        bool _isDashing = false;
        bool _isInteracting = false;
        bool _isInvincible = false;
        bool _isParrying = false;
        Animator _animator;
        ObjectPool<PlayerAfterImageSprite> _afterImagePool;
        PlayerBrain _playerBrain;

        [SerializeField] List<Ability> _abilities = new List<Ability>();
        [SerializeField] PlayerAfterImageSprite _afterImage;
        [SerializeField] float _dashTime = .1f;

        public Character Character;
        public PlayerAimWeapon Aim;

        #region getters & setters
        public ObjectPool<PlayerAfterImageSprite> AfterImagePool { get { return _afterImagePool; } }
        public float DashTime { get { return _dashTime; } }
        public Rigidbody2D Rb { get { return _rb; } }
        public bool IsDashing { get { return _isDashing; } set { _isDashing = value; } }
        public bool IsInteracting { get { return _isInteracting; } set { _isInteracting = value; } }
        public bool IsInvincible { get { return _isInvincible; } set { _isInvincible = value; } }
        public bool IsParrying { get { return _isParrying; } set { _isParrying = value; } }
        public Vector2 MovementInput { get { return _movementInput; } set { _movementInput = value; } }
        #endregion

        void Awake()
        {
            _playerBrain = new PlayerBrain(this);

            _afterImagePool = new ObjectPool<PlayerAfterImageSprite>(CreateAfterImage, 10, "AfterImagePool");

            for (int i = 0; i < _abilities.Count; i++)
            {
                _abilities[i] = Instantiate(_abilities[i]);
                _abilities[i].Initialize(gameObject);
            }

            Character.OnTakeDamageEvent += TakeDamage;
            Character.OnHealEvent += OnHeal;
            Character.OnDeathEvent += OnDeath;
            Character.OnStunEvent += OnStun;
            Character.OnExitStunEvent += OnExitStun;

            _animator = GetComponent<Animator>();
            _rb = GetComponent<Rigidbody2D>();
        }
        private void Start()
        {
            PlayerUI.SetHealthSlider(Character.Stats["Hp"].Get());
            PlayerUI.SetStaminaValue((int)Character.Stats["Stamina"].Get());
        }

        private void Update()
        {
            foreach (Ability ability in _abilities)
            {
                ability.UpdateAbility(Time.deltaTime);
            }
        }

        void FixedUpdate()
        {
            _playerBrain.UpdateBrain();

            if (_movementInput == Vector2.zero) { _animator.Play("Idle"); return; }
            _animator.Play("Walking");
        }

        public void Move(InputAction.CallbackContext context)
        {
            _movementInput = context.ReadValue<Vector2>() * Character.Stats["MoveSpeed"].Value;
        }

        public void Dash(int abilityIndex, InputAction.CallbackContext context)
        {
            if (context.performed && !_isDashing)
            {
                _abilities[abilityIndex].Cast();
            }
        }

        public void Parry(int abilityIndex, InputAction.CallbackContext context)
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

        public void OnStun()
        {
            if (_isInvincible) return;
            _playerBrain._FSM.SetState(PlayerBrain.PlayerStates.Staggered);
        }

        public void OnExitStun()
        {
            _playerBrain._FSM.SetState(PlayerBrain.PlayerStates.Default);
        }

        public void TakeDamage(float damage)
        {
            if (_isInvincible) return;
            Camera.main.Shake(0.1f, 0.2f);
            PlayerUI.SetHealthSlider(Character.Stats["Hp"].Get());
        }

        public void OnDeath()
        {
        }

        public void OnHeal(float amount)
        {
        }

        public void UsedStamina(int amount)
        {
            Character.Stats["Stamina"].Value -= amount;
            PlayerUI.SetStaminaValue((int)Character.Stats["Stamina"].Get());
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