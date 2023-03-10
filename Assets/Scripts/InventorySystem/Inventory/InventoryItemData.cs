using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.InventorySystem
{
    public class InventoryItemData : ScriptableObject
    {
        public int ID { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public Sprite Icon { get; set; }
        public Sprite Sprite { get; set; }
        public int MaxStackSize { get; set; }
        public int StackSize { get; set; }
        public Rarity Rarity { get; set; }
        public ItemType ItemType { get; set; }
    }
    public enum Rarity
    {
        Common,
        Rare,
        Epic,
        Legendary
    }

    public enum ItemType
    {
        Default,
        Weapon,
        Consumable
    }
}