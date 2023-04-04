using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BulletHell.InventorySystem
{
    [System.Serializable]
    public class InventoryHolder : MonoBehaviour
    {
        #region Private Fields
        [SerializeField] int _inventorySize;
        [SerializeField] protected InventorySystem _inventorySystem;
        #endregion

        #region Public Fields
        public InventorySystem InventorySystem => _inventorySystem;

        public static UnityAction<InventorySystem> OnDynamicInventoryDisplayRequested;
        #endregion

        #region Private Methods
        void Awake()
        {
            _inventorySystem = new InventorySystem(_inventorySize);
        }

        void OnEnable()
        {
            foreach(InventorySlot slot in _inventorySystem.InventorySlots)
            {
                slot?.OnInventorySlotAssigned?.Raise(null, slot.ItemData);
            }
        }
        #endregion
    }
}