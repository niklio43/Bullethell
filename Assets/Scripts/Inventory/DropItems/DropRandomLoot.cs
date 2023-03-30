using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.InventorySystem;

public class DropRandomLoot : Singleton<DropRandomLoot>, IDropLoot
{
    #region Private Fields
    [SerializeField] Material _droppedItemMaterial;
    #endregion

    #region Public Methods
    public void DropItem(List<ItemDrop> dropTable, Transform tf)
    {
        if(dropTable == null) { return; }
        var item = SelectRandomItem(dropTable);
        if (item == null) return;

        for (int i = 0; i < item.Count; i++)
        {
            InventoryItemData newItem = Instantiate(item[i]);

            GameObject droppedItem = new GameObject(newItem.DisplayName);

            droppedItem.transform.position = new Vector3(tf.position.x + Random.Range(-1.5f, 1.5f), tf.position.y + Random.Range(-3f, 3f));

            SpriteRenderer sr = droppedItem.AddComponent<SpriteRenderer>();
            sr.sprite = newItem.Sprite;
            sr.material = _droppedItemMaterial;

            DroppedItem di = droppedItem.AddComponent<DroppedItem>();
            di.ItemData = newItem;

            Collider2D col = droppedItem.AddComponent<CircleCollider2D>();
            col.isTrigger = true;

            droppedItem.layer = 6;
            sr.sortingLayerID = SortingLayer.NameToID("Top");
        }
    }
    #endregion

    #region Private Methods
    List<InventoryItemData> SelectRandomItem(List<ItemDrop> dropTable)
    {
        List<InventoryItemData> itemsToDrop = new List<InventoryItemData>();
        for (int i = 0; i < dropTable.Count; i++)
        {
            if(Random.Range(0f, 1f) <= dropTable[i].Chance)
            {
                itemsToDrop.Add(dropTable[i].Item);
            }
        }
        return itemsToDrop;
    }
    #endregion
}
