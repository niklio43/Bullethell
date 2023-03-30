using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Abilities;
using BulletHell.Emitters;

namespace BulletHell.InventorySystem
{
    [CreateAssetMenu(fileName = "New Melee Weapon", menuName = "Inventory System/Item/Weapon/Melee/Spear")]
    public class Spear : Melee
    {
        #region Private Fields
        [SerializeField] Sprite icon;
        [SerializeField] Sprite sprite;
        [SerializeField] string itemName;
        [TextArea(4, 20), SerializeField] string description;
        [SerializeField] int maxStackSize;
        [SerializeField] Pool pool;
        [SerializeField] Rarity rarity;
        [SerializeField] ItemType itemType;
        #endregion

        #region Private Methods
        void OnEnable()
        {
            Icon = icon;
            Sprite = sprite;
            DisplayName = itemName;
            Description = description;
            MaxStackSize = maxStackSize;
            Pool = pool;
            Rarity = rarity;
            ItemType = itemType;
        }
        #endregion
    }
}