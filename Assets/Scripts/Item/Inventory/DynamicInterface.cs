using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DynamicInterface : UserInterface
{
    //public int X_START;
    //public int Y_START;
    //public int X_SPACE_BETWEEN_ITEM;
    //public int NUMBER_OF_COLUMN;
    //public int Y_SPACE_BETWEEN_ITEMS;
    public GameObject ItemPrefab;

    public override void CreateSlots()
    {
        SlotsOnInterface = new Dictionary<GameObject, InventorySlot>();
        for (int i = 0; i < Inventory.GetSlots.Length; i++)
        {
            var obj = Instantiate(ItemPrefab, Vector3.zero, Quaternion.identity, transform);
            //obj.GetComponent<RectTransform>().localPosition = GetPosition(i);

            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragStart(obj); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });

            Inventory.GetSlots[i].SlotDisplay = obj;

            SlotsOnInterface.Add(obj, Inventory.GetSlots[i]);
        }
    }

    //Vector3 GetPosition(int i)
    //{
    //    return new Vector3(X_START + (X_SPACE_BETWEEN_ITEM * ((i % NUMBER_OF_COLUMN) -1)), Y_START + (-Y_SPACE_BETWEEN_ITEMS * ((i / NUMBER_OF_COLUMN)) -1), 0f);
    //}
}
