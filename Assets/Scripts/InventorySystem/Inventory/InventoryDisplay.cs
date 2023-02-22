using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace BulletHell.InventorySystem
{
    public abstract class InventoryDisplay : MonoBehaviour
    {
        [SerializeField] MouseObj _mouseObj;
        private InventorySlotUI sender;

        protected InventorySystem _inventorySystem;
        protected Dictionary<InventorySlotUI, InventorySlot> _slotDictionary;

        #region Getter
        public InventorySystem InventorySystem => _inventorySystem;
        public Dictionary<InventorySlotUI, InventorySlot> SlotDictionary => _slotDictionary;
        #endregion

        protected virtual void Start() { }

        public abstract void AssignSlot(InventorySystem invToDisplay);

        protected virtual void UpdateSlot(InventorySlot updatedSlot)
        {
            foreach (var slot in _slotDictionary)
            {
                if (slot.Value == updatedSlot)
                {
                    slot.Key.UpdateUISlot(updatedSlot);
                }
            }
        }

        public void SlotClicked(InventorySlotUI clickedUISlot)
        {
            _mouseObj.UpdateMouseSlot(clickedUISlot.AssignedInventorySlot);
            sender = clickedUISlot;
            return;
        }

        public void SlotReleased(InventorySlotUI targetUISlot, PointerEventData eventData)
        {
            if (_mouseObj.AssignedInventorySlot.ItemData != null)
            {
                sender.AssignedInventorySlot?.ClearSlot();

                InventorySlot previous = new InventorySlot();
                previous.AssignItem(targetUISlot.AssignedInventorySlot);

                targetUISlot.AssignedInventorySlot.AssignItem(_mouseObj.AssignedInventorySlot);

                sender.AssignedInventorySlot.AssignItem(previous);

                targetUISlot.SwapItems(eventData);
            }

            _mouseObj.ClearSlot();
        }

        public void DropItem(InventorySlotUI invSlot)
        {
            InventorySlot temp = new InventorySlot();
            temp.AssignItem(invSlot.AssignedInventorySlot);

            invSlot.ClearSlot();
            _mouseObj.ClearSlot();

            for (int i = 0; i < temp.StackSize; i++)
            {
                GameObject droppedItem = new GameObject();
                droppedItem.AddComponent<DroppedItem>();
                droppedItem.GetComponent<DroppedItem>().Initialize(temp.ItemData);

                //Temporary
                Transform player = GameObject.FindGameObjectWithTag("Player").transform;
                droppedItem.transform.position = new Vector2(player.position.x + Random.Range(-2, 2), player.position.y + Random.Range(-2, 2));
            }
        }

        public void ResetSlot()
        {
            _mouseObj.ClearSlot();
        }
    }
}