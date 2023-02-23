using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BulletHell.InventorySystem
{
    public class InventoryUIController : MonoBehaviour
    {
        public DynamicInventoryDisplay InventoryPanel;

        private void Awake()
        {
            InventoryPanel.gameObject.SetActive(true);
        }

        private void OnEnable()
        {
            InventoryHolder.OnDynamicInventoryDisplayRequested += DisplayInventory;
        }

        private void OnDisable()
        {
            InventoryHolder.OnDynamicInventoryDisplayRequested -= DisplayInventory;
        }

        void DisplayInventory(InventorySystem invToDisplay)
        {
            InventoryPanel.RefreshDynamicInventory(invToDisplay);
        }
    }
}