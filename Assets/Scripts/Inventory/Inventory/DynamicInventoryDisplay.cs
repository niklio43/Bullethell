using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace BulletHell.InventorySystem
{
    public class DynamicInventoryDisplay : InventoryDisplay
    {
        #region Private Fields
        [SerializeField] protected List<InventorySlotUI> _slotsUI;
        #endregion

        #region Private Methods
        protected override void Start()
        {
            base.Start();
        }

        private void ClearSlots()
        {
            if (_slotDictionary != null)
            {
                _slotDictionary.Clear();
            }
        }
        #endregion

        #region Public Methods
        public void RefreshDynamicInventory(InventorySystem invToDisplay)
        {
            ClearSlots();
            _inventorySystem = invToDisplay;
            AssignSlot(invToDisplay);
        }

        public override void AssignSlot(InventorySystem invToDisplay)
        {
            _slotDictionary = new Dictionary<InventorySlotUI, InventorySlot>();

            if (invToDisplay == null) { return; }

            for (int i = 0; i < invToDisplay.InventorySize; i++)
            {
                _slotDictionary.Add(_slotsUI[i], invToDisplay.InventorySlots[i]);
                _slotsUI[i].Initialize(invToDisplay.InventorySlots[i]);
                _slotsUI[i].UpdateUISlot();
            }
        }
        #endregion
    }
}