using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BulletHell.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        Rigidbody2D _rb;
        Animator _animator;
        Vector2 _movementInput;

        #region Getters & Setters
        public Rigidbody2D Rb { get { return _rb; } }
        public Vector2 MovementInput { get { return _movementInput; } set { _movementInput = value; } }
        #endregion

        void Awake()
        {
            _animator = GetComponent<Animator>();
            _rb = GetComponent<Rigidbody2D>();
        }

        void FixedUpdate()
        {
            if (_movementInput == Vector2.zero) { _animator.Play("Idle"); return; }
            _animator.Play("Walking");
        }

        public void Move(InputAction.CallbackContext context)
        {
            _movementInput = context.ReadValue<Vector2>() * GetComponent<PlayerController>().Character.Stats["MoveSpeed"].Value;
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