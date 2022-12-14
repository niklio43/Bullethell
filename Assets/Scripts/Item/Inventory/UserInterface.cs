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
    public PlayerInteracter Player;
    public Inventory Inventory;
    public Dictionary<GameObject, InventorySlot> _itemsDisplayed = new Dictionary<GameObject, InventorySlot>();

    void Start()
    {
        for (int i = 0; i < Inventory.Container.Items.Length; i++)
        {
            Inventory.Container.Items[i].parent = this;
        }
        CreateSlots();
        AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { OnEnterInterface(gameObject); });
        AddEvent(gameObject, EventTriggerType.PointerExit, delegate { OnExitInterface(gameObject); });
    }

    void Update()
    {
        UpdateSlots();
    }

    public abstract void CreateSlots();

    public void UpdateSlots()
    {
        foreach (KeyValuePair<GameObject, InventorySlot> slot in _itemsDisplayed)
        {
            if (slot.Value.ID >= 0)
            {
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = Inventory.database.GetItem[slot.Value.Item.Id].Sprite;
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
        Player.MouseItem.hoverObj = obj;
        if (_itemsDisplayed.ContainsKey(obj))
            Player.MouseItem.hoverItem = _itemsDisplayed[obj];
    }
    public void OnExit(GameObject obj)
    {
        Player.MouseItem.hoverObj = null;
        Player.MouseItem.hoverItem = null;
    }
    public void OnEnterInterface(GameObject obj)
    {
        Player.MouseItem.Ui = obj.GetComponent<UserInterface>();
    }
    public void OnExitInterface(GameObject obj)
    {
        Player.MouseItem.Ui = null;
    }
    public void OnDragStart(GameObject obj)
    {
        var mouseObject = new GameObject();
        var rt = mouseObject.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(50, 50);
        mouseObject.transform.SetParent(transform.parent);
        if (_itemsDisplayed[obj].ID >= 0)
        {
            var img = mouseObject.AddComponent<Image>();
            img.sprite = Inventory.database.GetItem[_itemsDisplayed[obj].ID].Sprite;
            img.raycastTarget = false;
        }
        Player.MouseItem.obj = mouseObject;
        Player.MouseItem.item = _itemsDisplayed[obj];
    }
    public void OnDragEnd(GameObject obj)
    {
        var itemOnMouse = Player.MouseItem;
        var mouseHoverItem = itemOnMouse.hoverItem;
        var mouseHoverObj = itemOnMouse.hoverObj;
        var getItemObject = Inventory.database.GetItem;

        if (itemOnMouse.Ui != null)
        {
            if (mouseHoverObj)
                if (mouseHoverItem.CanPlaceInSlot(getItemObject[_itemsDisplayed[obj].ID]) && (mouseHoverItem.Item.Id <= -1 || (mouseHoverItem.Item.Id >= 0 && _itemsDisplayed[obj].CanPlaceInSlot(getItemObject[mouseHoverItem.Item.Id]))))
                    Inventory.MoveItem(_itemsDisplayed[obj], mouseHoverItem.parent._itemsDisplayed[itemOnMouse.hoverObj]);
        }
        else
        {
            Inventory.RemoveItem(_itemsDisplayed[obj].Item); //functionallity to remove items when dragged out of inventory
        }
        Destroy(itemOnMouse.obj);
        itemOnMouse.item = null;
    }
    public void OnDrag(GameObject obj)
    {
        if (Player.MouseItem.obj != null)
            Player.MouseItem.obj.GetComponent<RectTransform>().position = Input.mousePosition;
    }

}

public class MouseItem
{
    public UserInterface Ui;
    public GameObject obj;
    public InventorySlot item;
    public InventorySlot hoverItem;
    public GameObject hoverObj;
}