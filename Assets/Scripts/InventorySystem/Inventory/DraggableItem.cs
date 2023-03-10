using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BulletHell.InventorySystem
{
    public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [HideInInspector] public Transform parentAfterDrag;
        [HideInInspector] public bool IsDragging = false;

        void Awake()
        {
            parentAfterDrag = transform.parent;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            IsDragging = true;
            parentAfterDrag = transform.parent;
            transform.SetParent(transform.root);
            transform.SetAsLastSibling();
            GetComponent<Image>().raycastTarget = false;

            parentAfterDrag.GetComponent<InventorySlotUI>().OnUISlotClick();

            HoverInfoManager.Instance.HideInfo();
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            IsDragging = false;
            transform.SetParent(parentAfterDrag);

            if (parentAfterDrag.GetComponent<InventorySlotUI>().AssignedInventorySlot.ItemData == null) { GetComponent<Image>().raycastTarget = true; return; }

            var tags = IsPointerOverUIObject(eventData);

            if (tags.Contains("Slot")) { GetComponent<Image>().raycastTarget = true; return; }

            if (tags.Count > 0 && tags != null)
            {
                parentAfterDrag.GetComponent<InventorySlotUI>().ResetSlot();
            }

            if (tags.Count <= 0 || tags == null)
            {
                parentAfterDrag.GetComponent<InventorySlotUI>().DropItem();
            }

            GetComponent<Image>().raycastTarget = true;
        }

        public static List<string> IsPointerOverUIObject(PointerEventData eventData)
        {
            eventData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            List<string> tags = new List<string>();
            for (int i = 0; i < results.Count; i++)
            {
                if (!string.IsNullOrEmpty(results[i].gameObject.tag))
                    tags.Add(results[i].gameObject.tag);
            }

            return tags;
        }
    }
}