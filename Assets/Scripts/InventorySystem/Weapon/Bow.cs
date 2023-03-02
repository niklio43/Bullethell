using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Abilities;
using BulletHell.Emitters;

namespace BulletHell.InventorySystem
{
    [CreateAssetMenu(fileName = "New Ranged Weapon", menuName = "Inventory System/Item/Weapon/Ranged/AssultRifle")]
    public class Bow : Ranged
    {
        [SerializeField] Sprite icon;
        [SerializeField] Sprite sprite;
        [SerializeField] string itemName;
        [SerializeField] Pool pool;
        [SerializeField] Rarity rarity;
        [SerializeField] ItemType itemType;

        void OnEnable()
        {
            Sprite = sprite;
            DisplayName = itemName;
            Pool = pool;
            Rarity = rarity;
            ItemType = itemType;
            Icon = icon;
        }
    }
}