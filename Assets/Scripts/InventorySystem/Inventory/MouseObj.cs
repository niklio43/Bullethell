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

        public void UpdateMouseSlot(InventorySlot invSlot)
        {
            AssignedInventorySlot?.AssignItem(invSlot);
        }

        public void ClearSlot()
        {
            AssignedInventorySlot = null;
        }
    }
}