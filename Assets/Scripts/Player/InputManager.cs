using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System;
using TMPro;

public class InputManager : MonoBehaviour
{
    public static PlayerInputs InputActions;

    public static event Action RebindComplete;
    public static event Action RebindCanceled;
    public static event Action<InputAction, int> RebindStarted;

    void Awake()
    {
        if (InputActions == null)
            InputActions = new PlayerInputs();

        DontDestroyOnLoad(gameObject);
    }

    public static void StartRebind(string actionName, int bindingIndex, TMP_Text statusText, bool excludeMouse)
    {
        InputAction action = InputActions.asset.FindAction(actionName);

        if(action == null || action.bindings.Count <= bindingIndex) { Debug.Log("Couldn't find action or binding"); return; }

        if (action.bindings[bindingIndex].isComposite)
        {
            var firstPartIndex = bindingIndex + 1;
            if (firstPartIndex < action.bindings.Count && action.bindings[firstPartIndex].isComposite)
                DoRebind(action, bindingIndex, statusText, true, excludeMouse);
        }
        else
        {
            DoRebind(action, bindingIndex, statusText, false, excludeMouse);
        }
    }

    static void DoRebind(InputAction actionToRebind, int bindingIndex, TMP_Text statusText, bool allCompositeParts, bool excludeMouse)
    {
        if (actionToRebind == null || bindingIndex < 0) return;

        statusText.text = $"Press a {actionToRebind.expectedControlType}";

        actionToRebind.Disable();

        var rebind = actionToRebind.PerformInteractiveRebinding(bindingIndex);

        rebind.OnComplete(operation =>
        {
            actionToRebind.Enable();
            operation.Dispose();

            if (allCompositeParts)
            {
                var nextBindingIndex = bindingIndex + 1;
                if (nextBindingIndex < actionToRebind.bindings.Count && actionToRebind.bindings[nextBindingIndex].isComposite)
                    DoRebind(actionToRebind, nextBindingIndex, statusText, allCompositeParts, excludeMouse);
            }

            SaveBindingOverride(actionToRebind);
            RebindComplete?.Invoke();
        });

        rebind.OnCancel(operation =>
        {
            actionToRebind.Enable();
            operation.Dispose();

            RebindCanceled?.Invoke();
        });

        rebind.WithCancelingThrough("<Keyboard>/escape");

        if (excludeMouse)
            rebind.WithControlsExcluding("Mouse");

        RebindStarted?.Invoke(actionToRebind, bindingIndex);
        rebind.Start(); //starts the rebinding process
    }

    public static string GetBindingName(string actionName, int bindingIndex)
    {
        if (InputActions == null)
            InputActions = new PlayerInputs();

        InputAction action = InputActions.asset.FindAction(actionName);
        return action.GetBindingDisplayString(bindingIndex);
    }

    static void SaveBindingOverride(InputAction action)
    {
        for (int i = 0; i < action.bindings.Count; i++)
        {
            PlayerPrefs.SetString(action.actionMap + action.name + i, action.bindings[i].overridePath);
        }
    }

    public static void LoadBindingOverride(string actionName)
    {
        if (InputActions == null)
            InputActions = new PlayerInputs();

        InputAction action = InputActions.asset.FindAction(actionName);

        for (int i = 0; i < action.bindings.Count; i++)
        {
            if (!string.IsNullOrEmpty(PlayerPrefs.GetString(action.actionMap + action.name + i)))
                action.ApplyBindingOverride(i, PlayerPrefs.GetString(action.actionMap + action.name + i));
        }
    }

    public static void ResetBinding(string actionName, int bindingIndex)
    {
        InputAction action = InputActions.asset.FindAction(actionName);

        if(action == null || action.bindings.Count <= bindingIndex) { Debug.Log("Couldn't find action or binding"); return; }

        if (action.bindings[bindingIndex].isComposite)
        {
            for (int i = bindingIndex; i < action.bindings.Count && action.bindings[i].isComposite; i++)
            {
                action.RemoveBindingOverride(i);
            }
        }
        else
        {
            action.RemoveBindingOverride(bindingIndex);
        }

        SaveBindingOverride(action);
    }
}
