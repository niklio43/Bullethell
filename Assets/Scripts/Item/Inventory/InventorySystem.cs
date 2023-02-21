using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

namespace BulletHell.InventorySystem
{
    [System.Serializable]
    public class InventorySystem
    {
        [SerializeField] List<InventorySlot> _inventorySlots;

        #region Getter
        public List<InventorySlot> InventorySlots => _inventorySlots;
        public int InventorySize => InventorySlots.Count;
        #endregion

        public UnityAction<InventorySlot> OnInventorySlotChanged;

        public InventorySystem(int size)
        {
            _inventorySlots = new List<InventorySlot>(size);

            for (int i = 0; i < size; i++)
            {
                _inventorySlots.Add(new InventorySlot());
            }
        }

        public bool AddToInventory(InventoryItemData itemToAdd, int amountToAdd)
        {
            if (ContainsItem(itemToAdd, out List<InventorySlot> invSlot)) // Check if item exists in inventory.
            {
                foreach (var slot in invSlot)
                {
                    if (slot.RoomLeftInStack(amountToAdd))
                    {
                        slot.AddToStack(amountToAdd);
                        OnInventorySlotChanged?.Invoke(slot);
                        return true;
                    }
                }
            }

            if (HasFreeSlot(out InventorySlot freeSlot)) // Gets first available slot.
            {
                freeSlot.UpdateInventorySlot(itemToAdd, amountToAdd);
                OnInventorySlotChanged?.Invoke(freeSlot);
                return true;
            }
            return false;
        }

        public bool ContainsItem(InventoryItemData itemToAdd, out List<InventorySlot> invSlot)
        {
            invSlot = InventorySlots.Where(i => i.ItemData == itemToAdd).ToList(); // Creates list of inventory slots and fills it where the ItemData is equal to the item that we want to add.
            return invSlot == null ? false : true;
        }

        public bool HasFreeSlot(out InventorySlot freeSlot)
        {
            freeSlot = InventorySlots.FirstOrDefault(i => i.ItemData == null); // Get first slot that is null or empty
            return freeSlot == null ? false : true;
        }
    }
}