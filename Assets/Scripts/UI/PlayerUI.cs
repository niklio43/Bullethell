using BulletHell.StatusSystem;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using BulletHell.Stats;
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
        [SerializeField] StatManagerUI _statManagerUI;
        public static HealthBar Health;
        public static StaminaBar Stamina;
        [Header("Forge")]
        public GameObject Forge;
        [SerializeField] Slider _forgeProgressBar;
        [SerializeField] TextMeshProUGUI _currentHealth;
        [SerializeField] PlayerController _playerController;
        [HideInInspector] public bool IsUpgrading = false;

        bool inv = false;

        protected override void OnAwake()
        {
            Health = _healthBar;
            Stamina = _staminaBar;
        }

        public void Initialize(Stats.Stats stats)
        {
            _statManagerUI.Initilize(stats);
            Health.Initialize(stats["Hp"]);
            Stamina.Initialize(stats["Stamina"]);
        }

        private void Start()
        {
            Inventory.SetActive(false);
        }

        private void Update()
        {
            _currentHealth.text = string.Concat("Blood: ", _playerController.Character.Stats["Hp"].Get());

            if (!IsUpgrading) { _forgeProgressBar.value = 0; return; }
            _forgeProgressBar.value += Time.deltaTime;
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

        public void AddStatusEffect(StatusEffect statusEffect)
        {
            _statusEffectUIManager.AddStatusEffect(statusEffect);
        }
    }
}