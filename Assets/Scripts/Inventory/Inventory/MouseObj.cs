using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BulletHell.InventorySystem
{
    public class MouseObj : MonoBehaviour
    {
        #region Public Fields
        public InventorySlot AssignedInventorySlot;
        public InventorySlotUI Sender;
        #endregion

        #region Public Methods
        public void UpdateMouseSlot(InventorySlotUI invSlotUI)
        {
            Sender = invSlotUI;
            AssignedInventorySlot?.AssignItem(Sender.AssignedInventorySlot.ItemData);
            AssignedInventorySlot.OnInventorySlotAssigned = Sender.AssignedInventorySlot.OnInventorySlotAssigned;
        }

        public void ClearSlot()
        {
            Sender = null;
            AssignedInventorySlot.ClearSlot();
            AssignedInventorySlot.OnInventorySlotAssigned = null;
        }
        #endregion
    }
}