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
        [SerializeField] GameObject _infoWindowPrefab;
        [Header("Stats and statuseffects")]
        [SerializeField] HealthBar _healthBar;
        [SerializeField] StaminaBar _staminaBar;
        [SerializeField] StatusEffectUIManager _statusEffectUIManager;

        public static HealthBar Health;
        public static StaminaBar Stamina;

        [Header("Forge")]
        public GameObject Forge;
        [SerializeField] TextMeshProUGUI _bloodCost;
        [SerializeField] Slider _forgeProgressBar;
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

        void Start()
        {
            Forge.SetActive(false);
            Inventory.SetActive(false);
            HoverInfoManager.Instance.InfoWindow = Instantiate(_infoWindowPrefab, transform).GetComponent<HoverInfoUI>();
        }

        void Update()
        {
            if(_forgeProgressBar == null) { return; }
            if(!IsUpgrading) { _forgeProgressBar.value = 0; return; }
            _forgeProgressBar.value += Time.deltaTime;
        }

        public void SetCost(float amount)
        {
            _bloodCost.text = string.Concat("Cost: ", amount);
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
            HoverInfoManager.Instance.HideInfo();
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