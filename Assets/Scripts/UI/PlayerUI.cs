using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerUI : Singleton<PlayerUI>
{
    [SerializeField] PlayerInput _input;
    [SerializeField] GameObject[] _inventory;
    [SerializeField] Transform _statusEffectHolder;
    [SerializeField] GameObject _statusEffect;
    public Slider Health, Stamina;
    bool inv = false;

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

    public static void SetMaxValueHud(Slider slider, float value)
    {
        slider.maxValue = value;
    }

    public static void SetValueHud(Slider slider, float value)
    {
        slider.value = value;
    }

    public void AddStatusEffect(float duration, float stacks, Sprite icon)
    {
        Transform iconObj = Instantiate(_statusEffect, _statusEffectHolder).transform;
        iconObj.GetComponent<Image>().sprite = icon;
        iconObj.GetChild(0).GetComponent<TMP_Text>().text = stacks.ToString();
        MonoInstance.Instance.StartCoroutine(RemoveStatusEffect(stacks, duration, iconObj));
    }

    IEnumerator RemoveStatusEffect(float stacks, float duration, Transform iconObj)
    {
        yield return new WaitForSeconds(duration);

        if (stacks <= 1) { Destroy(iconObj.gameObject); yield return null; }

        iconObj.GetChild(0).GetComponent<TMP_Text>().text = (stacks - 1).ToString();
    }

}
