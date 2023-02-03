using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Player;

public class SlotListener : MonoBehaviour
{
    public Inventory Inventory;
    public Inventory Equipment;
    [SerializeField] PlayerController _player;
    [SerializeField] WeaponController _weaponController;

    void Start()
    {
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
                    _player.Character.Stats.RemoveModifierFromStat(slot.Item.buffs[i]);
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
                    _player.Character.Stats.AddModifierToStat(slot.Item.buffs[i]);
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
