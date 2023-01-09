using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class RebindUI : MonoBehaviour
{
    [SerializeField] InputActionReference _inputActionReference;
    [SerializeField] bool _excludeMouse = true;
    [SerializeField, Range(0, 10)] int _selectedBinding;
    [SerializeField] InputBinding.DisplayStringOptions _displayStringOptions;

    [Header("Binding Info")]
    [SerializeField] InputBinding _inputBinding;
    int _bindingIndex;

    string _actionName;

    [Header("UI Fields")]
    [SerializeField] TMP_Text _actionText;
    [SerializeField] Button _rebindButton;
    [SerializeField] TMP_Text _rebindText;
    [SerializeField] Button _resetButton;

    void OnEnable()
    {
        _rebindButton.onClick.AddListener(() => DoRebind());
        _resetButton.onClick.AddListener(() => ResetBinding());

        if(_inputActionReference != null)
        {
            InputManager.LoadBindingOverride(_actionName);
            GetBindingInfo();
            UpdateUI();
        }

        InputManager.RebindComplete += UpdateUI;
        InputManager.RebindCanceled += UpdateUI;
    }

    private void OnDisable()
    {
        InputManager.RebindComplete -= UpdateUI;
        InputManager.RebindCanceled -= UpdateUI;
    }

    void OnValidate()
    {
        if (_inputActionReference == null) return;
        GetBindingInfo();
        UpdateUI();
    }

    void GetBindingInfo()
    {
        if(_inputActionReference.action != null)
            _actionName = _inputActionReference.action.name;

        if(_inputActionReference.action.bindings.Count > _selectedBinding)
            {
                _inputBinding = _inputActionReference.action.bindings[_selectedBinding];
                _bindingIndex = _selectedBinding;
            }
    }

    void UpdateUI()
    {
        if (_actionText != null)
            _actionText.text = _actionName;

        if(_rebindText != null)
        {
            if (Application.isPlaying)
            {
                _rebindText.text = InputManager.GetBindingName(_actionName, _bindingIndex);
            }
            else
            {
                _rebindText.text = _inputActionReference.action.GetBindingDisplayString(_bindingIndex);
            }
        }
    }

    void DoRebind()
    {
        InputManager.StartRebind(_actionName, _bindingIndex, _rebindText, _excludeMouse);
    }

    void ResetBinding()
    {
        InputManager.ResetBinding(_actionName, _bindingIndex);
        UpdateUI();
    }

}
