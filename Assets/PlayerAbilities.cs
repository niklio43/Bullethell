using BulletHell.Abilities;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BulletHell.Player
{
    public class PlayerAbilities : MonoBehaviour
    {
        bool _isDashing = false;
        bool _isInteracting = false;
        bool _isInvincible = false;
        bool _isParrying = false;

        ObjectPool<PlayerAfterImageSprite> _afterImagePool;

        [SerializeField] List<Ability> _abilities = new List<Ability>();
        [SerializeField] PlayerAfterImageSprite _afterImage;
        [SerializeField] float _dashTime = .1f;

        #region Getters & Setters
        public ObjectPool<PlayerAfterImageSprite> AfterImagePool { get { return _afterImagePool; } }
        public float DashTime { get { return _dashTime; } }
        public bool IsDashing { get { return _isDashing; } set { _isDashing = value; } }
        public bool IsInteracting { get { return _isInteracting; } set { _isInteracting = value; } }
        public bool IsInvincible { get { return _isInvincible; } set { _isInvincible = value; } }
        public bool IsParrying { get { return _isParrying; } set { _isParrying = value; } }
        #endregion

        void Awake()
        {
            _afterImagePool = new ObjectPool<PlayerAfterImageSprite>(CreateAfterImage, 10, "AfterImagePool");

            for (int i = 0; i < _abilities.Count; i++)
            {
                _abilities[i] = Instantiate(_abilities[i]);
                _abilities[i].Initialize(gameObject);
            }
        }

        void Update()
        {
            foreach (Ability ability in _abilities)
            {
                ability.UpdateAbility(Time.deltaTime);
            }
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
            if (context.performed && !_isParrying)
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