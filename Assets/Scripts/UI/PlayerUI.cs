using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
public class PlayerUI : MonoBehaviour
{
    [SerializeField] Transform[] HotBar;
    [SerializeField] PlayerInput _input;

    void Start()
    {
        foreach(Transform obj in HotBar)
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
            HotBar[0].GetChild(1).gameObject.GetComponent<ItemUI>().ConsumeItem();
        }
    }
    public void RightHotBar(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            HotBar[1].GetChild(1).gameObject.GetComponent<ItemUI>().ConsumeItem();
        }
    }
    public void BottomHotBar(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            HotBar[2].GetChild(1).gameObject.GetComponent<ItemUI>().ConsumeItem();
        }
    }
    public void TopHotBar(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            HotBar[3].GetChild(1).gameObject.GetComponent<ItemUI>().ConsumeItem();
        }
    }
}