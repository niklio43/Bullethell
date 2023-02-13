using System;
using UnityEngine;
using System.Collections.Generic;
using Bullet.CameraUtilities;
using BulletHell.StatusSystem;

namespace BulletHell.Player
{
    public class PlayerController : MonoBehaviour
    {
        PlayerBrain _playerBrain;
        PlayerMovement _playerMovement;
        PlayerAbilities _playerAbilities;

        public Character Character;
        public PlayerAimWeapon Aim;

        #region Getters & Setters
        public PlayerMovement PlayerMovement { get { return _playerMovement; } }
        public PlayerAbilities PlayerAbilities { get { return _playerAbilities; } }
        #endregion

        void Awake()
        {
            _playerBrain = new PlayerBrain(this);
            _playerMovement = GetComponent<PlayerMovement>();
            _playerAbilities = GetComponent<PlayerAbilities>();

            Character.OnTakeDamageEvent += TakeDamage;
            Character.OnHealEvent += OnHeal;
            Character.OnDeathEvent += OnDeath;
            Character.OnStunEvent += OnStun;
            Character.OnExitStunEvent += OnExitStun;
            Character.OnAppliedEffectEvent += OnAppliedStatusEffect;
            Character.OnRemovedEffectEvent += OnRemovedStatusEffect;
        }

        private void Start()
        {
            PlayerUI.SetHealthSlider(Character.Stats["Hp"].Get());
            PlayerUI.SetStaminaValue((int)Character.Stats["Stamina"].Get());
        }

        void FixedUpdate()
        {
            _playerBrain.UpdateBrain();
        }

        public void OnAppliedStatusEffect(StatusEffect statusEffect)
        {
            PlayerUI.Instance.AddStatusEffect(statusEffect);
        }

        public void OnRemovedStatusEffect(StatusEffect statusEffect)
        {
            PlayerUI.Instance.RemoveStatusEffect(statusEffect);
        }

        public void OnStun()
        {
            if (_playerAbilities.IsInvincible) return;
            _playerBrain._FSM.SetState(PlayerBrain.PlayerStates.Staggered);
        }

        public void OnExitStun()
        {
            _playerBrain._FSM.SetState(PlayerBrain.PlayerStates.Default);
        }

        public void TakeDamage(float damage)
        {
            if (_playerAbilities.IsInvincible) return;
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