using BulletHell.StatusSystem;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerUI : Singleton<PlayerUI>
{
    [SerializeField] PlayerInput _input;
    [SerializeField] GameObject _inventory;
    [SerializeField] Transform _statusEffectHolder;
    [SerializeField] GameObject _statusEffect;

    [SerializeField] Slider _healthSlider;
    [SerializeField] StaminaBar _staminaBar;


    public static Slider Health;
    public static StaminaBar Stamina;
    bool inv = false;

    List<StatusEffectIcon> effects = new List<StatusEffectIcon>();

    protected override void OnAwake()
    {
        Health = _healthSlider;
        Stamina = _staminaBar;
    }

    private void Start()
    {
        _inventory.SetActive(false);
    }

    public bool TryGetCurrentInputForAction(string action, out string input)
    {
        int binding = _input.actions[action].GetBindingIndex(group: _input.currentControlScheme);
        _input.actions[action].GetBindingDisplayString(binding, out string device, out input);
        return !string.IsNullOrEmpty(input);
    }

    public void ToggleInventory()
    {
        _inventory.SetActive(!_inventory.activeSelf);
    }

    public static void SetHealthSlider(float value)
    {
        if (value > Health.maxValue) { Health.maxValue = value; }
        Health.value = value;
    }

    public static void SetStaminaValue(int value)
    {
        Stamina.UpdateBar(value);
    }

    public void AddStatusEffect(StatusEffect statusEffect)
    {
        Transform iconObj = Instantiate(_statusEffect, _statusEffectHolder).transform;
        iconObj.GetComponent<Image>().sprite = statusEffect.Icon;

        effects.Add(new StatusEffectIcon(statusEffect, iconObj.gameObject));
    }

    public void RemoveStatusEffect(StatusEffect statusEffect)
    {
        if (statusEffect.GetStackCount() <= 1) {
            foreach (StatusEffectIcon icon in effects) {
                if (icon.StatusEffect == statusEffect) {
                    effects.Remove(icon);
                    icon.Destroy();
                    return;
                }
            }
        }
    }

    void Update()
    {
        foreach (StatusEffectIcon icon in effects) {
            icon.Update();
        }
    }

}

public class StatusEffectIcon
{
    public StatusEffect StatusEffect;
    GameObject _obj;

    TMP_Text _text;
    Image _image;

    public StatusEffectIcon(StatusEffect statusEffect, GameObject iconObj)
    {
        StatusEffect = statusEffect;
        _obj = iconObj;

        _text = _obj.transform.GetChild(1).GetComponent<TMP_Text>();
        _image = _obj.transform.GetChild(0).GetComponent<Image>();

    }

    public void Update()
    {
        _text.text = StatusEffect.GetStackCount().ToString();
        _image.fillAmount = 1 - (StatusEffect.GetTimerCount() / StatusEffect.LifeTime);
    }

    public void Destroy()
    {
        GameObject.Destroy(_obj);
    }
}