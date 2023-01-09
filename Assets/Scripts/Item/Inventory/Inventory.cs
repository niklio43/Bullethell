using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;
using System.Runtime.Serialization;

public enum InterfaceType
{
    Inventory,
    Equipment,
    Dialogue
}

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Item/Inventory")]
public class Inventory : ScriptableObject
{
    public string savePath;
    public ItemDatabase database;
    public InterfaceType type;
    public InventoryContainer Container;
    public InventorySlot[] GetSlots { get { return Container.Slots; } }

    public bool AddItem(ItemObject item, int amount)
    {
        if (EmptySlotCount <= 0) return false;
        InventorySlot slot = FindItemOnInventory(item);
        if(!database.Items[item.Id].Stackable || slot == null)
        {
            SetEmptySlot(item, amount);
            return true;
        }
        slot.AddAmount(amount);
        return true;
    }

    public int EmptySlotCount
    {
        get
        {
            int counter = 0;
            for (int i = 0; i < GetSlots.Length; i++)
            {
                if (GetSlots[i].Item.Id <= -1)
                    counter++;
            }
            return counter;
        }
    }

    public InventorySlot FindItemOnInventory(ItemObject itemObject)
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].Item.Id == itemObject.Id) return GetSlots[i];
        }
        return null;
    }

    public InventorySlot SetEmptySlot(ItemObject itemObject, int amount)
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].Item.Id <= -1)
            {
                GetSlots[i].UpdateSlot(itemObject, amount);
                return GetSlots[i];
            }
        }
        //TODO Add functionality for full inventory.
        return null;
    }

    public void SwapItems(InventorySlot item1, InventorySlot item2)
    {
        if (item2.CanPlaceInSlot(item1.GetItemData) && item1.CanPlaceInSlot(item2.GetItemData))
        {
            InventorySlot temp = new InventorySlot(item2.Item, item2.Amount);
            item2.UpdateSlot(item1.Item, item1.Amount);
            item1.UpdateSlot(temp.Item, temp.Amount);
        }
    }

    public void RemoveItem(ItemObject item)
    {
        for (int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i].Item == item)
            {
                GetSlots[i].UpdateSlot(null, 0);
            }
        }
    }

    [ContextMenu("Save")]
    public void Save()
    {
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);
        formatter.Serialize(stream, Container);
        stream.Close();
    }

    [ContextMenu("Load")]
    public void Load()
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Open, FileAccess.Read);
            InventoryContainer newContainer = (InventoryContainer)formatter.Deserialize(stream);
            for (int i = 0; i < GetSlots.Length; i++)
            {
                GetSlots[i].UpdateSlot(newContainer.Slots[i].Item, newContainer.Slots[i].Amount);
            }
            stream.Close();
        }
    }

    [ContextMenu("Clear")]
    public void Clear()
    {
        Container.Clear();
    }
}

[System.Serializable]
public class InventoryContainer
{
    public InventorySlot[] Slots = new InventorySlot[24];
    public void Clear()
    {
        for (int i = 0; i < Slots.Length; i++)
        {
            Slots[i].RemoveItem();
        }
    }
}

public delegate void SlotUpdated(InventorySlot slot);

[System.Serializable]
public class InventorySlot
{
    public ItemType[] AllowedItems = new ItemType[0];
    [System.NonSerialized] public UserInterface Parent;
    [System.NonSerialized] public GameObject SlotDisplay;
    [System.NonSerialized] public SlotUpdated OnAfterUpdate;
    [System.NonSerialized] public SlotUpdated OnBeforeUpdate;
    public ItemObject Item;
    public int Amount;

    public Item GetItemData
    {
        get
        {
            if (Item.Id >= 0)
            {
                return Parent.Inventory.database.Items[Item.Id];
            }
            return null;
        }
    }

    public InventorySlot()
    {
        UpdateSlot(new ItemObject(), 0);
    }
    public InventorySlot(ItemObject item, int amount)
    {
        UpdateSlot(item, amount);
    }
    public void UpdateSlot(ItemObject item, int amount)
    {
        if (OnBeforeUpdate != null)
            OnBeforeUpdate.Invoke(this);
        Item = item;
        Amount = amount;
        if (OnAfterUpdate != null)
            OnAfterUpdate.Invoke(this);
    }

    public void RemoveItem()
    {
        UpdateSlot(new ItemObject(), 0);
    }

    public void AddAmount(int value)
    {
        UpdateSlot(Item, Amount += value);
    }
    public bool CanPlaceInSlot(Item item)
    {
        if (AllowedItems.Length <= 0 || item == null || item.data.Id < 0)
            return true;
        for (int i = 0; i < AllowedItems.Length; i++)
        {
            if (item.ItemType == AllowedItems[i])
                return true;
        }
        return false;
    }
}