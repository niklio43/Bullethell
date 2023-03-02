using BulletHell.StatusSystem;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using BulletHell.Player;

namespace BulletHell.UI
{
    public class PlayerUI : Singleton<PlayerUI>
    {
        [Header("Key bindings")]
        [SerializeField] PlayerInput _input;
        [Header("Inventory")]
        public GameObject Inventory;
        [Header("Stats and statuseffects")]
        [SerializeField] HealthBar _healthBar;
        [SerializeField] StaminaBar _staminaBar;
        [SerializeField] StatusEffectUIManager _statusEffectUIManager;

        public static HealthBar Health;
        public static StaminaBar Stamina;

        [Header("Forge")]
        public GameObject Forge;
        [SerializeField] Slider _forgeProgressBar;
        [SerializeField] TextMeshProUGUI _currentHealth;
        [SerializeField] PlayerController _playerController;
        [HideInInspector] public bool IsUpgrading = false;

        public static void Initialize(PlayerController playerController)
        {
            Health.Initialize(playerController.PlayerResources);
            Stamina.Initialize(playerController.PlayerResources);
        }

        protected override void OnAwake()
        {
            Health = _healthBar;
            Stamina = _staminaBar;
        }

        private void Start()
        {
            Inventory.SetActive(false);
        }

        public bool TryGetCurrentInputForAction(string action, out string input)
        {
            int binding = _input.actions[action].GetBindingIndex(group: _input.currentControlScheme);
            _input.actions[action].GetBindingDisplayString(binding, out string device, out input);
            return !string.IsNullOrEmpty(input);
        }

        public void ToggleInventory()
        {
            Inventory.SetActive(!Inventory.activeSelf);
            Forge.SetActive(false);
        }

        public void AddStatusEffect(ActiveStatusEffect statusEffect)
        {
            _statusEffectUIManager.AddStatusEffect(statusEffect);
        }

        public void RemoveStatusEffect(ActiveStatusEffect statusEffect)
        {
            _statusEffectUIManager.RemoveStatusEffect(statusEffect);
        }

    }
}