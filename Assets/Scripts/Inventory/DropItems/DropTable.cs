using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Drop Table", menuName = "Inventory System/Item/Drop Table/Table")]
public class DropTable : ScriptableObject
{
    #region Private Fields
    [SerializeField] string _tableName;
    [SerializeField] List<ItemDrop> _itemDrop;
    #endregion

    #region Public Fields
    public string TableName => _tableName;
    public List<ItemDrop> ItemDrop => _itemDrop;
    #endregion
}