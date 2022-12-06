using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
public class PlayerUI : MonoBehaviour
{
    [SerializeField] Transform[] _hotbar;
    [SerializeField] PlayerInput _input;
    [SerializeField] GameObject _inventory;
    bool inv = false;

    void Start()
    {
        foreach(Transform obj in _hotbar)
        {
            string output;
            TryGetCurrentInputForAction(obj.name, out output);
            obj.GetChild(0).gameObject.GetComponent<TMP_Text>().text = output;
        }
    }

    public bool TryGetCurrentInputForAction(string action, out string input)
    {
        int binding = _input.actions[action].GetBindingIndex(group: _input.currentControlScheme);
        _input.actions[action].GetBindingDisplayString(binding, out string device, out input);
        return !string.IsNullOrEmpty(input);
    }

    public void LeftHotBar(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _hotbar[0].GetChild(1).gameObject.GetComponent<ItemUI>().ConsumeItem();
        }
    }
    public void RightHotBar(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _hotbar[1].GetChild(1).gameObject.GetComponent<ItemUI>().ConsumeItem();
        }
    }
    public void BottomHotBar(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _hotbar[2].GetChild(1).gameObject.GetComponent<ItemUI>().ConsumeItem();
        }
    }
    public void TopHotBar(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _hotbar[3].GetChild(1).gameObject.GetComponent<ItemUI>().ConsumeItem();
        }
    }

    public void ToggleInventory()
    {
        inv = !inv;
        _inventory.SetActive(inv);
    }
}
