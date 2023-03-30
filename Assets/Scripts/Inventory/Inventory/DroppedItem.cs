using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace BulletHell.InventorySystem
{
    public class DroppedItem : InteractableItem
    {
        #region Public Fields
        public InventoryItemData ItemData;
        #endregion

        #region Private Methods
        void Awake()
        {
            if (ItemData == null) { return; }
            Initialize(ItemData);
        }
        #endregion

        #region Public Methods
        public void Initialize(InventoryItemData itemData)
        {
            ItemData = Instantiate(itemData);
            gameObject.AddComponent<SpriteRenderer>().sprite = ItemData.Sprite;
            gameObject.AddComponent<CircleCollider2D>().isTrigger = true;
            gameObject.name = ItemData.name;
            GetComponent<SpriteRenderer>().sortingOrder = 1;
            gameObject.layer = LayerMask.NameToLayer("Interactable");
        }

        public override void Interact(InventorySystem inventory, PlayerResources playerResources)
        {
            if (ItemData.ItemType == ItemType.Consumable) { Use(playerResources); return; }
            if (inventory.AddToInventory(ItemData, 1))
            {
                Destroy(gameObject);
            }
        }

        public void Use(PlayerResources playerResources)
        {
            Consumables consumable = ItemData as Consumables;
            consumable.Use(playerResources);
            Destroy(gameObject);
        }
        #endregion
    }
}