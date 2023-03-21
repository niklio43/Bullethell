using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDropLoot
{
    void DropItem(List<ItemDrop> dropTable, Transform tf);
}
