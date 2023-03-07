using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using BulletHell.InventorySystem;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HoverInfoManager : Singleton<HoverInfoManager>
{
    [SerializeField] GameObject _infoWindow;

    public delegate void OnMouseHoverDelegate(InventoryItemData item);
    public OnMouseHoverDelegate OnMouseHover;
    public delegate void OnMouseLoseFocusDelegate();
    public OnMouseLoseFocusDelegate OnMouseLoseFocus;

    void Start()
    {
        _infoWindow = Instantiate(_infoWindow);
        HideInfo();
    }

    public void ShowInfo(InventoryItemData data)
    {
        if(data == null) { HideInfo(); return; }
        var infoUI = _infoWindow.GetComponent<HoverInfoUI>();
        infoUI.Icon.sprite = data.Icon;
        infoUI.ItemName.text = data.DisplayName;
        infoUI.ItemType.text = data.ItemType.ToString();
        infoUI.ItemRarity.text = data.Rarity.ToString();
        _infoWindow.SetActive(true);

        Vector2 mousePos = Mouse.current.position.ReadValue();
        _infoWindow.transform.position = new Vector2(mousePos.x - ((_infoWindow.GetComponent<RectTransform>().sizeDelta.x / 2) + 5),
            mousePos.y + (_infoWindow.GetComponent<RectTransform>().sizeDelta.y / 4));
    }

    public void HideInfo()
    {
        var infoUI = _infoWindow.GetComponent<HoverInfoUI>();
        infoUI.Icon.sprite = default;
        infoUI.ItemName.text = default;
        infoUI.ItemType.text = default;
        infoUI.ItemRarity.text = default;
        _infoWindow.SetActive(false);
    }
}
