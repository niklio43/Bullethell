using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Drop Table", menuName = "Inventory System/Item/Drop Table/Table")]
public class DropTable : ScriptableObject
{
    [SerializeField] string _tableName;
    [SerializeField] List<ItemDrop> _itemDrop;

    public string TableName => _tableName;
    public List<ItemDrop> ItemDrop => _itemDrop;
}