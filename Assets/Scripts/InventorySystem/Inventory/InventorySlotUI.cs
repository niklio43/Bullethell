using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

namespace BulletHell.InventorySystem
{
    public class InventorySlotUI : MonoBehaviour, IDropHandler
    {
        [SerializeField] Image _itemSprite;
        [SerializeField] TextMeshProUGUI _itemCount;
        [SerializeField] InventorySlot _assignedInventorySlot;
        [SerializeField] ItemType _allowedItems;

        #region Getter
        public InventoryDisplay ParentDisplay { get; private set; }
        public ItemType AllowedItems => _allowedItems;
        public InventorySlot AssignedInventorySlot => _assignedInventorySlot;
        #endregion

        void Awake()
        {
            ClearSlot();

            ParentDisplay = transform.parent.GetComponent<InventoryDisplay>();
        }

        public void Initialize(InventorySlot slot)
        {
            var unityEvent = _assignedInventorySlot.OnAssign;
            _assignedInventorySlot = slot;
            _assignedInventorySlot.OnAssign = unityEvent;

            UpdateUISlot(slot);
        }

        public void UpdateUISlot(InventorySlot slot)
        {
            if (slot.ItemData != null)
            {
                _itemSprite.sprite = slot.ItemData.Icon;
                _itemSprite.color = Color.white;
                if (slot.ItemData.StackSize > 1) { _itemCount.text = slot.ItemData.StackSize.ToString(); }
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
            _itemSprite.sprite = null;
            _itemSprite.color = Color.clear;
            _itemCount.text = "";

            _assignedInventorySlot?.ClearSlot();
        }

        public void OnDrop(PointerEventData eventData)
        {
            ParentDisplay?.SlotReleased(this, eventData);
        }

        public void OnUISlotClick()
        {
            ParentDisplay?.SlotClicked(this);
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
            currentObj.GetComponent<DraggableItem>().parentAfterDrag = draggableItem.parentAfterDrag;
            draggableItem.parentAfterDrag = transform;

            UpdateAssignedVariables(draggableItem);
            UpdateAssignedVariables(currentObj.GetComponent<DraggableItem>());
        }

        void UpdateAssignedVariables(DraggableItem item)
        {
            var slot = item.parentAfterDrag.GetComponent<InventorySlotUI>();

            slot._itemSprite = item.GetComponent<Image>();
            slot._itemCount = item.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        }
    }
}