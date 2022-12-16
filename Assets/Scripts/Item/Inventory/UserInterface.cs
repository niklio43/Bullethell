using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public abstract class UserInterface : MonoBehaviour
{
    public Inventory Inventory;
    public Dictionary<GameObject, InventorySlot> SlotsOnInterface = new Dictionary<GameObject, InventorySlot>();

    void Start()
    {
        for (int i = 0; i < Inventory.GetSlots.Length; i++)
        {
            Inventory.GetSlots[i].Parent = this;
            Inventory.GetSlots[i].OnAfterUpdate += OnSlotUpdate;
        }
        CreateSlots();
        AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { OnEnterInterface(gameObject); });
        AddEvent(gameObject, EventTriggerType.PointerExit, delegate { OnExitInterface(gameObject); });
    }

    private void OnSlotUpdate(InventorySlot slot)
    {
        if (slot.Item.Id >= 0)
        {
            slot.SlotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().sprite = slot.GetItemData.Sprite;
            slot.SlotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
            slot.SlotDisplay.GetComponentInChildren<TextMeshProUGUI>().text = slot.Amount == 1 ? "" : slot.Amount.ToString("n0");
        }
        else
        {
            slot.SlotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
            slot.SlotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
            slot.SlotDisplay.GetComponentInChildren<TextMeshProUGUI>().text = "";
        }
    }

    public abstract void CreateSlots();

    protected void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    public void OnEnter(GameObject obj)
    {
        MouseData.SlotHoveredOver = obj;
    }

    public void OnExit(GameObject obj)
    {
        MouseData.SlotHoveredOver = null;
    }

    public void OnEnterInterface(GameObject obj)
    {
        MouseData.InterfaceMouseIsOver = obj.GetComponent<UserInterface>();
    }

    public void OnExitInterface(GameObject obj)
    {
        MouseData.InterfaceMouseIsOver = null;
    }

    public void OnDragStart(GameObject obj)
    {
        MouseData.TempItemBeingDragged = CreateTempItem(obj);
    }

    public GameObject CreateTempItem(GameObject obj)
    {
        GameObject tempItem = null;
        if(SlotsOnInterface[obj].Item.Id >= 0)
        {
            tempItem = new GameObject();
            var rt = tempItem.AddComponent<RectTransform>();
            rt.sizeDelta = new Vector2(50, 50);
            tempItem.transform.SetParent(transform.parent);
            var img = tempItem.AddComponent<Image>();
            img.sprite = SlotsOnInterface[obj].GetItemData.Sprite;
            img.raycastTarget = false;
            img.preserveAspect = true;
        }
        return tempItem;
    }

    public void OnDragEnd(GameObject obj)
    {
        Destroy(MouseData.TempItemBeingDragged);
        if(MouseData.InterfaceMouseIsOver == null)
        {
            SlotsOnInterface[obj].RemoveItem();
            return;
        }
        if (MouseData.SlotHoveredOver)
        {
            InventorySlot mouseHoverSlotData = MouseData.InterfaceMouseIsOver.SlotsOnInterface[MouseData.SlotHoveredOver];
            Inventory.SwapItems(SlotsOnInterface[obj], mouseHoverSlotData);
        }
    }

    public void OnDrag(GameObject obj)
    {
        if (MouseData.TempItemBeingDragged != null)
            MouseData.TempItemBeingDragged.GetComponent<RectTransform>().position = Input.mousePosition;
    }
}

public static class MouseData
{
    public static UserInterface InterfaceMouseIsOver;
    public static GameObject TempItemBeingDragged;
    public static GameObject SlotHoveredOver;
}

public static class ExtensionMethods
{
    public static void UpdateSlotDisplay(this Dictionary<GameObject, InventorySlot> _slotsOnInterface)
    {
        foreach (KeyValuePair<GameObject, InventorySlot> slot in _slotsOnInterface)
        {
            if (slot.Value.Item.Id >= 0)
            {
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = slot.Value.GetItemData.Sprite;
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
                slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = slot.Value.Amount == 1 ? "" : slot.Value.Amount.ToString("n0");
            }
            else
            {
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
                slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
        }
    }
}