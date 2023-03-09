using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BulletHell.InventorySystem
{
    [System.Serializable]
    public class InventoryHolder : MonoBehaviour
    {
        [SerializeField] int _inventorySize;
        [SerializeField] protected InventorySystem _inventorySystem;

        #region Getter
        public InventorySystem InventorySystem => _inventorySystem;
        #endregion

        public static UnityAction<InventorySystem> OnDynamicInventoryDisplayRequested;

        void Awake()
        {
            _inventorySystem = new InventorySystem(_inventorySize);
        }

        void OnEnable()
        {
            foreach(InventorySlot slot in _inventorySystem.InventorySlots)
            {
                slot?.OnAssign?.Invoke(slot.ItemData);
            }
        }
    }
}