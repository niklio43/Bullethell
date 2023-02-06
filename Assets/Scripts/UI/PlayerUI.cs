using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] PlayerInput _input;
    [SerializeField] GameObject[] _inventory;
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
}
