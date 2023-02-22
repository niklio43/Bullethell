using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.InventorySystem
{
    [System.Serializable]
    public class InventorySlot
    {
        [SerializeField] InventoryItemData _itemData;
        [SerializeField] int _stackSize;

        #region Getter
        public InventoryItemData ItemData => _itemData;
        public int StackSize => _stackSize;
        #endregion

        public InventorySlot(InventoryItemData source, int amount)
        {
            _itemData = source;
            _stackSize = amount;
        }

        public InventorySlot()
        {
            ClearSlot();
        }

        public void ClearSlot()
        {
            _itemData = null;
            _stackSize = -1;
        }

        public void AssignItem(InventorySlot invSlot)
        {
            _itemData = invSlot.ItemData;
            _stackSize = invSlot.StackSize;
        }

        public void UpdateInventorySlot(InventoryItemData data, int amount)
        {
            _itemData = data;
            _stackSize = amount;
        }

        public bool RoomLeftInStack(int amountToAdd, out int amountRemaining)
        {
            amountRemaining = _itemData.MaxStackSize - _stackSize;
            return RoomLeftInStack(amountToAdd);
        }

        public bool RoomLeftInStack(int amountToAdd)
        {
            if (_stackSize + amountToAdd <= _itemData.MaxStackSize) { return true; }
            return false;
        }

        public void AddToStack(int amount)
        {
            _stackSize += amount;
        }

        public void RemoveFromStack(int amount)
        {
            _stackSize -= amount;
        }
    }
}