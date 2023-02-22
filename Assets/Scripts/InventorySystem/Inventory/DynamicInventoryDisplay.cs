using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace BulletHell.InventorySystem
{
    public class DynamicInventoryDisplay : InventoryDisplay
    {
        [SerializeField] protected InventorySlotUI slotPrefab;

        protected override void Start()
        {
            InventoryHolder.OnDynamicInventoryDisplayRequested += RefreshDynamicInventory;
            base.Start();

            AssignSlot(_inventorySystem);
        }

        private void OnDestroy()
        {
            InventoryHolder.OnDynamicInventoryDisplayRequested -= RefreshDynamicInventory;
        }

        public void RefreshDynamicInventory(InventorySystem invToDisplay)
        {
            _inventorySystem = invToDisplay;
        }

        public override void AssignSlot(InventorySystem invToDisplay)
        {
            ClearSlots();

            _slotDictionary = new Dictionary<InventorySlotUI, InventorySlot>();

            if(invToDisplay == null) { return; }

            for (int i = 0; i < invToDisplay.InventorySize; i++)
            {
                var uiSlot = Instantiate(slotPrefab, transform);
                _slotDictionary.Add(uiSlot, invToDisplay.InventorySlots[i]);
                uiSlot.Initialize(invToDisplay.InventorySlots[i]);
                uiSlot.UpdateUISlot();
            }
        }

        private void ClearSlots()
        {
            //kanske byta till object pooling?
            foreach (var item in transform.Cast<Transform>())
            {
                Destroy(item.gameObject);
            }

            if(_slotDictionary != null)
            {
                _slotDictionary.Clear();
            }
        }

    }
}