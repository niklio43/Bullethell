using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace BulletHell.InventorySystem
{
    public class DroppedItem : InteractableItem
    {
        public InventoryItemData ItemData;

        void Awake()
        {
            if(ItemData == null) { return; }
            Initialize(ItemData);
        }

        public void Initialize(InventoryItemData itemData)
        {
            ItemData = Instantiate(itemData);
            gameObject.AddComponent<SpriteRenderer>().sprite = ItemData.Sprite;
            gameObject.AddComponent<CircleCollider2D>().isTrigger = true;
            gameObject.name = ItemData.name;
            GetComponent<SpriteRenderer>().sortingOrder = 1;
            gameObject.layer = LayerMask.NameToLayer("Interactable");
        }

        public override void Interact(InventorySystem inventory)
        {
            if (inventory.AddToInventory(ItemData, 1))
            {
                Destroy(gameObject);
            }
        }
    }
}