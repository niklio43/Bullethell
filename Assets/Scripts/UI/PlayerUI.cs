using BulletHell.StatusSystem;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using BulletHell.Stats;

namespace BulletHell.UI
{
    public class PlayerUI : Singleton<PlayerUI>
    {
        [SerializeField] PlayerInput _input;
        public GameObject Inventory;
        [SerializeField] HealthBar _healthBar;
        [SerializeField] StaminaBar _staminaBar;
        [SerializeField] StatusEffectUIManager _statusEffectUIManager;
        [SerializeField] StatManagerUI _statManagerUI;

        public static HealthBar Health;
        public static StaminaBar Stamina;
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

        public bool TryGetCurrentInputForAction(string action, out string input)
        {
            int binding = _input.actions[action].GetBindingIndex(group: _input.currentControlScheme);
            _input.actions[action].GetBindingDisplayString(binding, out string device, out input);
            return !string.IsNullOrEmpty(input);
        }

        public void ToggleInventory()
        {
            Inventory.SetActive(!Inventory.activeSelf);
            ForgeUI.Instance.gameObject.SetActive(!ForgeUI.Instance.gameObject.activeSelf);
        }

        public void AddStatusEffect(StatusEffect statusEffect)
        {
            _statusEffectUIManager.AddStatusEffect(statusEffect);
        }
    }
}