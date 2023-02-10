using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using BulletHell.StatusSystem;

public class PlayerUI : Singleton<PlayerUI>
{
    [SerializeField] PlayerInput _input;
    [SerializeField] GameObject[] _inventory;
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


    public bool TryGetCurrentInputForAction(string action, out string input)
    {
        int binding = _input.actions[action].GetBindingIndex(group: _input.currentControlScheme);
        _input.actions[action].GetBindingDisplayString(binding, out string device, out input);
        return !string.IsNullOrEmpty(input);
    }

    public void ToggleInventory()
    {
        inv = !inv;
        foreach (GameObject obj in _inventory)
            obj.SetActive(inv);
    }

    public static void SetHealthSlider(float value)
    {
        if(value > Health.maxValue) { Health.maxValue = value; }
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
        if (statusEffect.GetStackCount() <= 1)
        {
            foreach (StatusEffectIcon icon in effects)
            {
                if (icon.StatusEffect == statusEffect)
                {
                    effects.Remove(icon);
                    Destroy(icon.Obj);
                    return;
                }
            }
        }
    }

    void Update()
    {
        foreach (StatusEffectIcon icon in effects)
        {
            //TODO cache.
            icon.Obj.transform.GetChild(1).GetComponent<TMP_Text>().text = icon.StatusEffect.GetStackCount().ToString();
            icon.Obj.transform.GetChild(0).GetComponent<Image>().fillAmount = 1 - (icon.StatusEffect.GetTimerCount() / icon.StatusEffect.LifeTime);
        }
    }

}

public class StatusEffectIcon
{
    public StatusEffect StatusEffect;
    public GameObject Obj;

    public StatusEffectIcon(StatusEffect statusEffect, GameObject iconObj)
    {
        StatusEffect = statusEffect;
        Obj = iconObj;
    }
}