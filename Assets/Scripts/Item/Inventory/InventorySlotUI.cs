using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

namespace BulletHell.InventorySystem
{
    public class InventorySlotUI : MonoBehaviour, IDropHandler
    {
        [SerializeField] Image _itemSprite;
        [SerializeField] TextMeshProUGUI _itemCount;
        [SerializeField] InventorySlot _assignedInventorySlot;

        #region Getter
        public InventoryDisplay ParentDisplay { get; private set; }
        public InventorySlot AssignedInventorySlot => _assignedInventorySlot;
        #endregion

        void Awake()
        {
            ClearSlot();

            ParentDisplay = transform.parent.GetComponent<InventoryDisplay>();
        }

        public void Initialize(InventorySlot slot)
        {
            _assignedInventorySlot = slot;
            UpdateUISlot(slot);
        }

        public void UpdateUISlot(InventorySlot slot)
        {
            if (slot.ItemData != null)
            {
                _itemSprite.sprite = slot.ItemData.Icon;
                _itemSprite.color = Color.white;
                if (slot.StackSize > 1) { _itemCount.text = slot.StackSize.ToString(); }
                else { _itemCount.text = ""; }
            }
            else
            {
                ClearSlot();
            }
        }

        public void UpdateUISlot()
        {
            if (_assignedInventorySlot != null) { UpdateUISlot(_assignedInventorySlot); }
        }

        public void ClearSlot()
        {
            _assignedInventorySlot?.ClearSlot();
            _itemSprite.sprite = null;
            _itemSprite.color = Color.clear;
            _itemCount.text = "";
        }

        public void OnDrop(PointerEventData eventData)
        {
            OnUISlotRelease(eventData);
        }

        public void OnUISlotClick()
        {
            ParentDisplay?.SlotClicked(this);
        }

        public void OnUISlotRelease(PointerEventData eventData)
        {
            ParentDisplay?.SlotReleased(this, eventData);
        }

        public void DropItem()
        {
            ParentDisplay?.DropItem(this);
        }

        public void ResetSlot()
        {
            ParentDisplay?.ResetSlot();
        }

        public void SwapItems(PointerEventData eventData)
        {
            Transform currentObj = transform.GetChild(0);

            GameObject dropped = eventData.pointerDrag;
            DraggableItem draggableItem = dropped.GetComponent<DraggableItem>();

            currentObj.SetParent(draggableItem.parentAfterDrag);
            draggableItem.parentAfterDrag = transform;
        }
    }
}