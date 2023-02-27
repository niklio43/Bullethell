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
        public InventorySlot AssignedInventorySlot;
        public InventorySlotUI Sender;

        public void UpdateMouseSlot(InventorySlotUI invSlotUI)
        {
            Sender = invSlotUI;
            AssignedInventorySlot?.AssignItem(Sender.AssignedInventorySlot.ItemData);
            AssignedInventorySlot.OnAssign = Sender.AssignedInventorySlot.OnAssign;
        }

        public void ClearSlot()
        {
            Sender = null;
            AssignedInventorySlot.ClearSlot();
            AssignedInventorySlot.OnAssign = null;
        }
    }
}