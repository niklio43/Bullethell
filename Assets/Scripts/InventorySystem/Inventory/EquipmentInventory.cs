using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.InventorySystem
{
    public class EquipmentInventory : InventoryHolder
    {
        private void OnEnable()
        {
            OnDynamicInventoryDisplayRequested?.Invoke(_inventorySystem);
        }
    }
}