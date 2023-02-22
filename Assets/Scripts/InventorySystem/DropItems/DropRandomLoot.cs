using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.InventorySystem;

/* How to use:
 * add this script to object that needs to drop random loot
 * make serialized list of itemdrop class like this ([SerializeField] List<ItemDrop> _dropTable = new List<ItemDrop>();)
 * choose items and drop chances in inspector (effectively making your own loot table)
 * getcomponent on this script and call DropItem method when desired, eg:
 * public void TakeDamage(float value)
 * {
 *  _stats.Health -= value;
 *  if(_stats.Health <= 0)
 *  {
 *      GetComponent<DropRandomLoot>().DropItem(_dropTable);
 *  }
 * }
 * obviously needs to be broken out in other methods like OnDeath() but this is only example.
 */

public class DropRandomLoot : MonoBehaviour, IDropLoot
{
    public void DropItem(List<ItemDrop> dropTable)
    {
        var item = SelectRandomItem(dropTable);
        if (item == null) return;

        for (int i = 0; i < item.Count; i++)
        {
            GameObject droppedItem = new GameObject(item[i].DisplayName);

            droppedItem.transform.position = new Vector3(transform.position.x + Random.Range(-1.5f, 1.5f), transform.position.y + Random.Range(-3f, 3f));

            SpriteRenderer sr = droppedItem.AddComponent<SpriteRenderer>();
            sr.sprite = item[i].Sprite;

            DroppedItem di = droppedItem.AddComponent<DroppedItem>();
            di.ItemData = item[i];

            Collider2D col = droppedItem.AddComponent<CircleCollider2D>();
            col.isTrigger = true;

            droppedItem.layer = 6;
            sr.sortingLayerID = SortingLayer.NameToID("Top");
        }
    }

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
}
