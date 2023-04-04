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
        #region Private Fields
        [Header("Key bindings")]
        [SerializeField] PlayerInput _input;
        [Header("Inventory")]
        [SerializeField] GameObject _inventory;
        [SerializeField] GameObject _infoWindowPrefab;
        [Header("Stats and statuseffects")]
        [SerializeField] HealthBar _healthBar;
        [SerializeField] StaminaBar _staminaBar;
        [SerializeField] StatusEffectUIManager _statusEffectUIManager;


        [Header("Forge")]
        [SerializeField] GameObject _forge;
        [SerializeField] TextMeshProUGUI _bloodCost;
        [SerializeField] Slider _forgeProgressBar;
        [SerializeField] PlayerController _playerController;
        bool _isUpgrading = false;

        [Header("Minimap")]
        [SerializeField] GameObject _largeMap;
        [SerializeField] Camera _minimapCamera;
        [SerializeField] RenderTexture _minimapTexture;
        [SerializeField] RenderTexture _largemapTexture;
        #endregion

        #region Public Fields
        public static HealthBar Health;
        public static StaminaBar Stamina;
        public GameObject Inventory => _inventory;
        public GameObject Forge => _forge;
        public bool IsUpgrading { get { return _isUpgrading; } set { _isUpgrading = value; } }
        public GameObject LargeMap => _largeMap;
        #endregion

        #region Private Methods
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
            if (_forgeProgressBar == null) { return; }
            if (!IsUpgrading) { _forgeProgressBar.value = 0; return; }
            _forgeProgressBar.value += Time.deltaTime;
        }
        #endregion

        #region Public Methods

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

        public void ToggleLargeMap()
        {
            //set new target to be center of entire map and zoom out camera
            bool active = !_largeMap.activeSelf;
            _largeMap.SetActive(active);

            MinimapCamera cam = _minimapCamera.GetComponent<MinimapCamera>();

            if (active)
            {
                cam.SetSize(10);
                cam.SetTexture(_largemapTexture);
                return;
            }
            cam.SetSize(1.5f);
            cam.SetTexture(_minimapTexture);
        }

        public void AddStatusEffect(ActiveStatusEffect statusEffect)
        {
            _statusEffectUIManager.AddStatusEffect(statusEffect);
        }

        public void RemoveStatusEffect(ActiveStatusEffect statusEffect)
        {
            _statusEffectUIManager.RemoveStatusEffect(statusEffect);
        }
        #endregion
    }
}