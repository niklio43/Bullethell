using System;
using UnityEngine;
using System.Collections.Generic;
using BulletHell.CameraUtilities;
using BulletHell.StatusSystem;
using BulletHell.UI;

namespace BulletHell.Player
{
    public class PlayerController : MonoBehaviour
    {
        #region Private Fields
        PlayerBrain _playerBrain;
        PlayerMovement _playerMovement;
        PlayerAbilities _playerAbilities;
        UnitStatusEffects _unitStatusEffects;
        PlayerResources _playerResources;
        [SerializeField] WeaponController _weaponController;
        float timer = 0;
        #endregion

        #region Public Fields
        public PlayerAimWeapon Aim;
        public PlayerMovement PlayerMovement => _playerMovement;
        public PlayerAbilities PlayerAbilities => _playerAbilities;
        public PlayerResources PlayerResources => _playerResources;
        public WeaponController WeaponController => _weaponController;
        #endregion

        #region Private Methods
        void Awake()
        {
            _playerBrain = new PlayerBrain(this);
            _playerMovement = GetComponent<PlayerMovement>();
            _playerAbilities = GetComponent<PlayerAbilities>();
            _playerResources = GetComponent<PlayerResources>();

            _unitStatusEffects = GetComponent<UnitStatusEffects>();
            _unitStatusEffects.OnAppliedStatusEffect += OnAppliedStatusEffect;
            _unitStatusEffects.OnRemovedStatusEffect += OnRemovedStatusEffect;
        }

        void FixedUpdate()
        {
            _playerBrain.UpdateBrain();
        }
        #endregion

        #region Public Methods
        public void OnAppliedStatusEffect(ActiveStatusEffect statusEffect)
        {
            PlayerUI.Instance.AddStatusEffect(statusEffect);
        }

        public void OnRemovedStatusEffect(ActiveStatusEffect statusEffect)
        {
            PlayerUI.Instance.RemoveStatusEffect(statusEffect);
        }

        public void OnStun()
        {
            //FIX THIS IS NOT LONGER BEING CALLED
            if (_playerAbilities.IsInvincible) return;
            _playerBrain._FSM.SetState(PlayerBrain.PlayerStates.Staggered);
        }

        public void OnExitStun()
        {
            //FIX SAME AS ABOVE
            _playerBrain._FSM.SetState(PlayerBrain.PlayerStates.Default);
        }

        public void TakeDamage(float damage)
        {
            //FIX THIS IS NOT LONGER BEING CALLED
            if (_playerAbilities.IsInvincible) return;
            Camera.main.Shake(0.1f, 0.2f);
        }

        public void OnDestroy()
        {
            _unitStatusEffects.OnAppliedStatusEffect -= OnAppliedStatusEffect;
            _unitStatusEffects.OnRemovedStatusEffect -= OnRemovedStatusEffect;
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