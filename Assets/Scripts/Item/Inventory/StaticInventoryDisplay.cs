using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.InventorySystem
{
    public class StaticInventoryDisplay : InventoryDisplay
    {
        [SerializeField] InventoryHolder _inventoryHolder;
        [SerializeField] InventorySlotUI[] _slots;

        protected override void Start()
        {
            base.Start();

            if (_inventoryHolder != null)
            {
                _inventorySystem = _inventoryHolder.InventorySystem;
                _inventorySystem.OnInventorySlotChanged += UpdateSlot;
            }
            else { Debug.LogWarning($"No inventory assigned to {this.gameObject}"); }

            AssignSlot(_inventorySystem);
        }
        public override void AssignSlot(InventorySystem invToDisplay)
        {
            _slotDictionary = new Dictionary<InventorySlotUI, InventorySlot>();

            if (_slots.Length != _inventorySystem.InventorySize) { Debug.LogWarning($"Inventory slots out of sync on {this.gameObject}"); }

            for (int i = 0; i < _inventorySystem.InventorySize; i++)
            {
                _slotDictionary.Add(_slots[i], _inventorySystem.InventorySlots[i]);
                _slots[i].Initialize(_inventorySystem.InventorySlots[i]);
            }
        }
    }
}