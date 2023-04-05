using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace BulletHell.InventorySystem
{
    using Random = UnityEngine.Random;

    public abstract class InventoryDisplay : MonoBehaviour
    {
        #region Private Fields
        [SerializeField] MouseObj _mouseObj;

        protected InventorySystem _inventorySystem;
        protected Dictionary<InventorySlotUI, InventorySlot> _slotDictionary;
        #endregion

        #region Public Fields
        public InventorySystem InventorySystem => _inventorySystem;
        public Dictionary<InventorySlotUI, InventorySlot> SlotDictionary => _slotDictionary;
        #endregion

        #region Private Methods
        protected virtual void Start() { }
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
        #endregion

        #region Public Methods
        public abstract void AssignSlot(InventorySystem invToDisplay);


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

                StartCoroutine(AnimateDrop(droppedItem.transform, player.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), .2f));

            }
        }

        IEnumerator AnimateDrop(Transform item, Vector3 startPos, Vector2 targetPos, float time)
        {
            item.position = startPos;
            float timeElapsed = 0;
            while (timeElapsed < time)
            {
                yield return new WaitForEndOfFrame();
                timeElapsed += Time.deltaTime;

                float tempPosX = Easing.EaseInOut(startPos.x, targetPos.x, timeElapsed / time);
                float tempPosY = Easing.EaseInOut(startPos.y, targetPos.y, timeElapsed / time);
                Vector3 finalPos = new Vector3(tempPosX, tempPosY, 0);

                float distanceMulti = Vector3.Distance(startPos, targetPos);
                item.position += (finalPos - startPos).normalized * Time.deltaTime * distanceMulti;
            }
        }

        public void ResetSlot()
        {
            _mouseObj.ClearSlot();
        }
        #endregion
    }
}