using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotListener : MonoBehaviour
{
    public Inventory Inventory;
    public Inventory Equipment;
    PlayerController _playerController;
    WeaponController _weaponController;

    void Start()
    {
        _playerController = transform.parent.GetComponent<PlayerController>();
        _weaponController = GetComponentInChildren<WeaponController>();

        for (int i = 0; i < Equipment.GetSlots.Length; i++)
        {
            Equipment.GetSlots[i].OnBeforeUpdate += OnBeforeSlotUpdate;
            Equipment.GetSlots[i].OnAfterUpdate += OnAfterSlotUpdate;
        }
    }

    public void OnBeforeSlotUpdate(InventorySlot slot)
    {
        if (slot.Item.Id == -1) return;

        switch (slot.Parent.Inventory.type)
        {
            case InterfaceType.Inventory:
                break;
            case InterfaceType.Equipment:
                for (int i = 0; i < slot.Item.buffs.Length; i++)
                {
                    _playerController.Stats.RemoveModifierFromStat(slot.Item.buffs[i]);
                }
                if (slot.GetItemData.ItemType == ItemType.Weapon && slot == Equipment.GetSlots[3])
                {
                    _weaponController.UnAssignWeapon(Equipment.GetSlots[3].GetItemData as Weapon);
                }
                break;
            case InterfaceType.Dialogue:
                break;
            default:
                break;
        }
    }

    public void OnAfterSlotUpdate(InventorySlot slot)
    {
        if (slot.Item.Id == -1) return;

        switch (slot.Parent.Inventory.type)
        {
            case InterfaceType.Inventory:
                break;
            case InterfaceType.Equipment:
                for (int i = 0; i < slot.Item.buffs.Length; i++)
                {
                    _playerController.Stats.AddModifierToStat(slot.Item.buffs[i]);
                }
                if (slot.GetItemData.ItemType == ItemType.Weapon && slot == Equipment.GetSlots[3])
                {
                    _weaponController.AssignWeapon(Equipment.GetSlots[3].GetItemData as Weapon);
                }
                break;
            case InterfaceType.Dialogue:
                break;
            default:
                break;
        }
    }
}
