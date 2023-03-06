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
    [SerializeField] Image _icon;
    [SerializeField] GameObject _infoWindow;
    [SerializeField] TextMeshProUGUI _itemName;
    [SerializeField] TextMeshProUGUI _itemType;
    [SerializeField] TextMeshProUGUI _itemRarity;

    public static Action<InventoryItemData> OnMouseHover;
    public static Action OnMouseLoseFocus;

    void OnEnable()
    {
        OnMouseHover += ShowInfo;
        OnMouseLoseFocus += HideInfo;
    }

    void OnDisable()
    {
        OnMouseHover -= ShowInfo;
        OnMouseLoseFocus -= HideInfo;
    }

    void Start()
    {
        HideInfo();
    }

    void ShowInfo(InventoryItemData data)
    {
        _icon.sprite = data.Icon;
        _itemName.text = data.DisplayName;
        _itemType.text = data.ItemType.ToString();
        _itemRarity.text = data.Rarity.ToString();
        _infoWindow.SetActive(true);

        Vector2 mousePos = Mouse.current.position.ReadValue();
        _infoWindow.transform.position = new Vector2(mousePos.x - ((_infoWindow.GetComponent<RectTransform>().sizeDelta.x / 2) + 5),
            mousePos.y + (_infoWindow.GetComponent<RectTransform>().sizeDelta.y / 4));
    }

    void HideInfo()
    {
        _itemName.text = "";
        _infoWindow.SetActive(false);
    }
}
