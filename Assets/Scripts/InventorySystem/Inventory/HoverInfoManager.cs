using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using BulletHell.InventorySystem;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using BulletHell.Abilities;

public class HoverInfoManager : Singleton<HoverInfoManager>
{
    [SerializeField] GameObject _infoWindow;

    void Start()
    {
        _infoWindow = Instantiate(_infoWindow);
        HideInfo();
    }

    public void ShowInfo(InventoryItemData data)
    {
        if (data == null) { HideInfo(); return; }
        var infoUI = _infoWindow.GetComponent<HoverInfoUI>();
        infoUI.Icon.sprite = data.Icon;
        infoUI.ItemName.text = data.DisplayName;
        infoUI.ItemType.text = data.ItemType.ToString();
        infoUI.ItemRarity.text = data.Rarity.ToString();
        _infoWindow.SetActive(true);

        Vector2 mousePos = Mouse.current.position.ReadValue();
        _infoWindow.transform.position = new Vector2(mousePos.x - ((_infoWindow.GetComponent<RectTransform>().sizeDelta.x / 2) + 5),
            mousePos.y + (_infoWindow.GetComponent<RectTransform>().sizeDelta.y / 4));

        foreach(Transform child in infoUI.AbilityParent.transform)
        {
            Destroy(child.gameObject);
        }
        if (data is Weapon)
        {
            Weapon weapon = data as Weapon;
            foreach (Ability ability in weapon.Abilities)
            {
                ShowAbilities(ability);
            }
            return;
        }
    }

    void ShowAbilities(Ability ability)
    {
        HoverInfoUI hoverInfoUI = _infoWindow.GetComponent<HoverInfoUI>();
        AbilityInfo abilityInfo = Instantiate(hoverInfoUI.AbilityPrefab, hoverInfoUI.AbilityParent.transform);

        abilityInfo.AbilityIcon.sprite = ability.GetIcon();
        abilityInfo.AbilityName.text = ability.GetName();
        abilityInfo.AbilityDescription.text = ability.GetDescription();
    }

    public void HideInfo()
    {
        var infoUI = _infoWindow.GetComponent<HoverInfoUI>();
        infoUI.Icon.sprite = default;
        infoUI.ItemName.text = default;
        infoUI.ItemType.text = default;
        infoUI.ItemRarity.text = default;

        foreach (Transform child in infoUI.AbilityParent.transform)
        {
            Destroy(child.gameObject);
        }

        _infoWindow.SetActive(false);
    }
}
