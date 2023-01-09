using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            GameObject droppedItem = new GameObject(item[i].ItemName);

            droppedItem.transform.position = new Vector3(transform.position.x + Random.Range(-3f, 3f), transform.position.y + Random.Range(-3f, 3f));
            droppedItem.transform.eulerAngles = new Vector3(0, 0, Random.Range(0, 360));

            SpriteRenderer sr = droppedItem.AddComponent<SpriteRenderer>();
            sr.sprite = item[i].Sprite;

            DroppedItem di = droppedItem.AddComponent<DroppedItem>();
            di._item = item[i];

            droppedItem.layer = 6;
        }
    }

    List<Item> SelectRandomItem(List<ItemDrop> dropTable)
    {
        List<Item> itemsToDrop = new List<Item>();
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
