using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace BulletHell.InventorySystem
{
    public class DroppedItem : MonoBehaviour, IPickUp, ISerializationCallbackReceiver
    {
        public InventoryItemData ItemData;

        public void AssignItem() { }

        public void Initialize(InventoryItemData itemData)
        {
            ItemData = itemData;
            gameObject.AddComponent<SpriteRenderer>().sprite = ItemData.Sprite;
            gameObject.AddComponent<CircleCollider2D>().isTrigger = true;
            gameObject.name = ItemData.name;
        }

        public void Interact(InventorySystem inventory)
        {
            if (inventory.AddToInventory(ItemData, 1))
            {
                Destroy(gameObject);
            }
        }

        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            GetComponent<SpriteRenderer>().sprite = ItemData.Sprite;
            EditorUtility.SetDirty(GetComponent<SpriteRenderer>());
#endif
        }


        public void OnAfterDeserialize() { }
    }
}