using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.InventorySystem;

[System.Serializable]
public class ItemDrop
{
    public InventoryItemData Item;
    [Range(0f, 1f)] public float Chance;
}
