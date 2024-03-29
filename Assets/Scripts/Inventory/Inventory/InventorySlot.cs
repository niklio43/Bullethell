using BulletHell.GameEventSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BulletHell.InventorySystem
{
    [System.Serializable]
    public class InventorySlot
    {
        #region Private Fields
        [SerializeField] InventoryItemData _itemData;
        #endregion

        #region Public Fields
        public InventoryItemData ItemData => _itemData;
        [Header("Events")]
        public SOGameEvent OnInventorySlotAssigned;
        #endregion

        #region Public Methods
        public InventorySlot(InventoryItemData source, int amount)
        {
            _itemData = source;
            _itemData.StackSize = amount;
        }

        public InventorySlot()
        {
            ClearSlot();
        }

        public void ClearSlot()
        {
            _itemData = null;
        }

        public void AssignItem(InventoryItemData data)
        {
            _itemData = data;

            OnInventorySlotAssigned?.Raise(null, _itemData);
        }

        public void UpdateInventorySlot(InventoryItemData data, int amount)
        {
            _itemData = data;
            _itemData.StackSize = amount;
        }

        public bool RoomLeftInStack(int amountToAdd, out int amountRemaining)
        {
            amountRemaining = _itemData.MaxStackSize - _itemData.StackSize;
            return RoomLeftInStack(amountToAdd);
        }

        public bool RoomLeftInStack(int amountToAdd)
        {
            if (_itemData.StackSize + amountToAdd <= _itemData.MaxStackSize) { return true; }
            return false;
        }

        public void AddToStack(int amount)
        {
            _itemData.StackSize += amount;
        }

        public void RemoveFromStack(int amount)
        {
            _itemData.StackSize -= amount;
        }
        #endregion
    }
}