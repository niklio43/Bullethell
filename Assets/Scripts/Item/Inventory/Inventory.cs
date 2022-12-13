using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;
using System.Runtime.Serialization;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Item/Inventory")]
public class Inventory : ScriptableObject
{
    public string savePath;
    public ItemDatabase database;
    public InventoryContainer Container;

    public void AddItem(ItemObject item, int amount)
    {
        if (database.GetItem[item.Id] is Weapon || database.GetItem[item.Id] is Armor)
        {
            SetEmptySlot(item, amount);
            return;
        }
        for (int i = 0; i < Container.Items.Length; i++)
        {
            if (Container.Items[i].ID == item.Id)
            {
                Container.Items[i].AddAmount(amount);
                return;
            }
        }
        SetEmptySlot(item, amount);
    }

    public InventorySlot SetEmptySlot(ItemObject itemObject, int amount)
    {
        for (int i = 0; i < Container.Items.Length; i++)
        {
            if(Container.Items[i].ID <= -1)
            {
                Container.Items[i].UpdateSlot(itemObject.Id, itemObject, amount);
                return Container.Items[i];
            }
        }
        //no space in inv
        return null;
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
        if(File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Open, FileAccess.Read);
            Container = (InventoryContainer)formatter.Deserialize(stream);
            stream.Close();
        }
    }

    [ContextMenu("Clear")]
    public void Clear()
    {
        Container = new InventoryContainer();
    }
}

[System.Serializable]
public class InventoryContainer
{
    public InventorySlot[] Items = new InventorySlot[24];
}

[System.Serializable]
public class InventorySlot
{
    public int ID = -1;
    public ItemObject Item;
    public int Amount;
    public InventorySlot()
    {
        ID = -1;
        Item = null;
        Amount = 0;
    }
    public InventorySlot(int _id, ItemObject item, int amount)
    {
        ID = _id;
        Item = item;
        Amount = amount;
    }
    public void UpdateSlot(int _id, ItemObject item, int amount)
    {
        ID = _id;
        Item = item;
        Amount = amount;
    }

    public void AddAmount(int value)
    {
        Amount += value;
    }
}