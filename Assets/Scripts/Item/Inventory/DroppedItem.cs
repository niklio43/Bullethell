using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class DroppedItem : MonoBehaviour, IPickUp, ISerializationCallbackReceiver
{
    public Item _item;

    public void AssignItem(){}

    public void Interact(Inventory inventory)
    {
        if(inventory.AddItem(new ItemObject(_item), 1))
        {
            Destroy(gameObject);
        }
    }

    public void OnBeforeSerialize()
    {
#if UNITY_EDITOR
        GetComponent<SpriteRenderer>().sprite = _item.Sprite;
        EditorUtility.SetDirty(GetComponent<SpriteRenderer>());
#endif
    }


    public void OnAfterDeserialize(){}
}