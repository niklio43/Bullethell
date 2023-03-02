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
    void Start()
    {
        PlayerUI.Instance.Forge.SetActive(false);

        _slotItem.AssignedInventorySlot.OnAssign += AssignWeapon;
    }

    public override void Interact(InventorySystem inventory)
    {
        PlayerUI.Instance.Forge.SetActive(true);
        PlayerUI.Instance.Inventory.SetActive(true);
    }

    public void AssignWeapon(InventoryItemData item)
    {
        _button.onClick.AddListener(delegate () { UpgradeWeapon(item); });
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

        if (_player.Health <= 50) { Debug.Log("Not enough blood!"); FailedUpgrade(); return; }

        WeaponAbility weaponAbility = weapon.Pool._ability[UnityEngine.Random.Range(0, weapon.Pool._ability.Length)];

        foreach (Ability ab in weapon.Abilities)
        {
            if (ab.Id == weaponAbility.Ability.Id) { FillAbilitySlot(weapon); return; }
        }
        foreach (Ability ab in weapon.AbilitySlot)
        {
            if (ab.Id == weaponAbility.Ability.Id) { FillAbilitySlot(weapon); return; }
        }

        StartCoroutine(BeginUpgrade(weapon, weaponAbility, _player.gameObject, _weapon.gameObject));
    }

    IEnumerator BeginUpgrade(Weapon weapon, WeaponAbility weaponAbility, GameObject owner, GameObject host)
    {
        PlayerUI.Instance.IsUpgrading = true;
        yield return new WaitForSeconds(1f);
        Debug.Log(string.Concat("Added ability: ", weaponAbility.Ability, " to weapon: ", weapon.DisplayName));
        PlayerUI.Instance.IsUpgrading = false;
        _player.Damage(new DamageValue(DamageType.Unblockable, weaponAbility.Cost));
        weapon.AddAbility(weaponAbility.Ability, owner, host);
    }

    void FailedUpgrade()
    {
        Camera.main.Shake(0.1f, 1f);
    }
}
