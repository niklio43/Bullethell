using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DroppedItem : MonoBehaviour, IPickUp
{
    public Item _item;

    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = _item.Sprite;
    }

    public void AssignItem()
    {
        throw new System.NotImplementedException();
    }

    public void Interact(Inventory inventory)
    {
        inventory.AddItem(new ItemObject(_item), 1);
        Destroy(gameObject);
    }
}