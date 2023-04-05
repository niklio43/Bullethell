using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.InventorySystem;
using BulletHell.UI;
using UnityEngine.UI;
using TMPro;
using BulletHell.Abilities;
using BulletHell.Player;
using BulletHell.CameraUtilities;
using BulletHell;

public class Forge : InteractableItem
{
    #region Private Fields
    [SerializeField] InventorySlotUI _slotItem;
    [SerializeField] Button _button;
    [SerializeField] WeaponController _weapon;
    [SerializeField] PlayerResources _player;

    WeaponAbility _abilityToAdd;
    #endregion

    #region Private Methods

    void SetRandomAbility(Weapon weapon)
    {
        WeaponAbility ability = weapon.Pool.Ability.Rand();

        foreach (Ability ab in weapon.Abilities)
        {
            if (ab.Id == ability.Ability.Id) { SetRandomAbility(weapon); return; }
        }
        if (weapon.BaseAbility.Id == ability.Ability.Id) { SetRandomAbility(weapon); return; }

        if (weapon.Abilities.Contains(ability.Ability)) { SetRandomAbility(weapon); return; }

        _abilityToAdd = ability;
    }

    void UpgradeWeapon(InventoryItemData item)
    {
        if (item == null) { return; }
        Debug.Log("Attempting upgrade");
        FillAbilitySlot(item as Weapon);
    }

    void FillAbilitySlot(Weapon weapon)
    {
        if (weapon.Abilities.Count >= 3) { Debug.Log("Too many abilities applied!"); FailedUpgrade(); return; }

        if (_player.Health <= _abilityToAdd.Cost) { Debug.Log("Not enough blood!"); FailedUpgrade(); return; }

        StartCoroutine(BeginUpgrade(weapon, _abilityToAdd, _player.gameObject, _weapon.gameObject));
    }

    IEnumerator BeginUpgrade(Weapon weapon, WeaponAbility weaponAbility, GameObject owner, GameObject host)
    {
        PlayerUI.Instance.IsUpgrading = true;
        _slotItem.transform.GetChild(0).GetComponent<Image>().raycastTarget = false;
        yield return new WaitForSeconds(1f);
        Debug.Log(string.Concat("Added ability: ", weaponAbility.Ability, " to weapon: ", weapon.DisplayName));
        PlayerUI.Instance.IsUpgrading = false;
        _slotItem.transform.GetChild(0).GetComponent<Image>().raycastTarget = true;
        Camera.main.Shake(0.05f, 0.5f);
        _player.Damage(new DamageValue(DamageType.Unblockable, weaponAbility.Cost));
        weapon.AddAbility(weaponAbility.Ability, owner, host);
        _abilityToAdd = null;
        _button.onClick.RemoveAllListeners();
    }

    void FailedUpgrade()
    {
        Camera.main.Shake(0.1f, 1f);
    }
    #endregion

    #region Public Methods
    public override void Interact(InventorySystem inventory, PlayerResources playerResources)
    {
        PlayerUI.Instance.Forge.SetActive(true);
        PlayerUI.Instance.Inventory.SetActive(true);
    }

    public void AssignWeaponToUpgrade(Component sender, object data)
    {
        if (data is not InventoryItemData) { return; }
        var item = data as InventoryItemData;

        if (item is Weapon)
        {
            _button.onClick.AddListener(delegate () { UpgradeWeapon(item); });

            if (item == null) { PlayerUI.Instance.SetCost(0); return; }
            Weapon weapon = item as Weapon;
            SetRandomAbility(weapon);
            PlayerUI.Instance.SetCost(_abilityToAdd.Cost);
            return;
        }
        _button.onClick.RemoveAllListeners();
    }
    #endregion
}
