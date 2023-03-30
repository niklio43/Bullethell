using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using BulletHell.InventorySystem;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using BulletHell.Abilities;
using BulletHell.UI;

public class HoverInfoManager : Singleton<HoverInfoManager>
{
    #region Private Fields
    HoverInfoUI _infoWindow;
    #endregion

    #region Public Fields
    public HoverInfoUI InfoWindow { get { return _infoWindow; } set { _infoWindow = value; } }
    #endregion

    #region Private Methods
    void Start()
    {
        HideInfo();
    }

    void ShowAbilities(Ability ability)
    {
        AbilityInfo abilityInfo = Instantiate(_infoWindow.AbilityPrefab, _infoWindow.AbilityParent.transform);

        abilityInfo.AbilityIcon.sprite = ability.GetIcon();
        abilityInfo.AbilityName.text = ability.GetName();
        abilityInfo.AbilityDescription.text = ability.GetDescription();
    }
    #endregion

    #region Public Methods
    public void ShowInfo(InventoryItemData data)
    {
        if (data == null) { HideInfo(); return; }
        _infoWindow.Icon.sprite = data.Icon;
        _infoWindow.ItemName.text = data.DisplayName;
        _infoWindow.ItemType.text = data.ItemType.ToString();
        _infoWindow.ItemRarity.text = data.Rarity.ToString();
        _infoWindow.gameObject.SetActive(true);

        Vector2 mousePos = Mouse.current.position.ReadValue();
        _infoWindow.transform.position = new Vector2(mousePos.x - ((_infoWindow.GetComponent<RectTransform>().sizeDelta.x / 2) + 5),
            mousePos.y + (_infoWindow.GetComponent<RectTransform>().sizeDelta.y / 4));

        foreach(Transform child in _infoWindow.AbilityParent.transform)
        {
            Destroy(child.gameObject);
        }
        if (data.ItemType == ItemType.Weapon)
        {
            Weapon weapon = data as Weapon;
            foreach (Ability ability in weapon.Abilities)
            {
                ShowAbilities(ability);
            }
            return;
        }
    }

    public void HideInfo()
    {
        _infoWindow.Icon.sprite = default;
        _infoWindow.ItemName.text = default;
        _infoWindow.ItemType.text = default;
        _infoWindow.ItemRarity.text = default;

        foreach (Transform child in _infoWindow.AbilityParent.transform)
        {
            Destroy(child.gameObject);
        }

        _infoWindow.gameObject.SetActive(false);
    }
    #endregion
}
