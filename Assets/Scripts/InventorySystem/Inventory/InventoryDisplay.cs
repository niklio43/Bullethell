using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace BulletHell.InventorySystem
{
    public abstract class InventoryDisplay : MonoBehaviour
    {
        [SerializeField] MouseObj _mouseObj;

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
            _mouseObj.UpdateMouseSlot(clickedUISlot);
            return;
        }

        public void SlotReleased(InventorySlotUI targetUISlot, PointerEventData eventData)
        {
            if (_mouseObj.Sender != null && _mouseObj.Sender != targetUISlot)
            {
                if (_mouseObj.Sender.AssignedInventorySlot.ItemData.ItemType == targetUISlot.AllowedItems
                    || targetUISlot.AllowedItems == ItemType.Default)
                {
                    targetUISlot.SwapItems(eventData);

                    _mouseObj.Sender.AssignedInventorySlot?.ClearSlot();

                    InventorySlot tempSlot1 = new InventorySlot();
                    tempSlot1.AssignItem(targetUISlot.AssignedInventorySlot.ItemData);

                    InventorySlot tempSlot2 = new InventorySlot();
                    tempSlot2.AssignItem(_mouseObj.AssignedInventorySlot.ItemData);

                    _mouseObj.Sender.AssignedInventorySlot.AssignItem(tempSlot1.ItemData);
                    targetUISlot.AssignedInventorySlot.AssignItem(tempSlot2.ItemData);
                }
            }



            ResetSlot();
        }

        public void DropItem(InventorySlotUI invSlot)
        {
            InventorySlot temp = new InventorySlot();
            temp.AssignItem(invSlot.AssignedInventorySlot.ItemData);

            invSlot.ClearSlot();
            invSlot.AssignedInventorySlot.AssignItem(null);
            ResetSlot();

            for (int i = 0; i < temp.ItemData.StackSize; i++)
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