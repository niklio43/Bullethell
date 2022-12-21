using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Database", menuName = "Inventory System/Item/Database")]
public class ItemDatabase : ScriptableObject, ISerializationCallbackReceiver
{
    public Item[] Items;

    [ContextMenu("Update ID's")]
    public void UpdateID()
    {
        for (int i = 0; i < Items.Length; i++)
        {
            if (Items[i].data.Id != i)
                Items[i].data.Id = i;
        }
    }

    public void OnAfterDeserialize()
    {
        UpdateID();
    }

    public void OnBeforeSerialize() { }
}
