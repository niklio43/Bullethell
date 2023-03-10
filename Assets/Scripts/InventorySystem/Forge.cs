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
    [SerializeField] InventorySlotUI _slotItem;
    [SerializeField] Button _button;
    [SerializeField] WeaponController _weapon;
    [SerializeField] PlayerResources _player;

    WeaponAbility _abilityToAdd;

    void Start()
    {
        _slotItem.AssignedInventorySlot.OnAssign += AssignWeaponToUpgrade;
    }

    public override void Interact(InventorySystem inventory, PlayerResources playerResources)
    {
        PlayerUI.Instance.Forge.SetActive(true);
        PlayerUI.Instance.Inventory.SetActive(true);
    }

    public void AssignWeaponToUpgrade(InventoryItemData item)
    {
        _button.onClick.AddListener(delegate () { UpgradeWeapon(item); });

        if(item == null) { PlayerUI.Instance.SetCost(0); return; }
        Weapon weapon = item as Weapon;
        _abilityToAdd = weapon.Pool._ability[UnityEngine.Random.Range(0, weapon.Pool._ability.Length)];
        PlayerUI.Instance.SetCost(_abilityToAdd.Cost);
    }

    void UpgradeWeapon(InventoryItemData item)
    {
        if (item == null) { return; }
        Debug.Log("Attempting upgrade");
        FillAbilitySlot(item as Weapon);
    }

    public void FillAbilitySlot(Weapon weapon)
    {
        if (weapon.Abilities.Count >= 3) { Debug.Log("Too many abilities applied!"); FailedUpgrade(); return; }

        if (_player.Health <= _abilityToAdd.Cost) { Debug.Log("Not enough blood!"); FailedUpgrade(); return; }

        foreach (Ability ab in weapon.Abilities)
        {
            if (ab.Id == _abilityToAdd.Ability.Id) { FillAbilitySlot(weapon); return; }
        }
        if (weapon.BaseAbility.Id == _abilityToAdd.Ability.Id) { FillAbilitySlot(weapon); return; }

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
}
