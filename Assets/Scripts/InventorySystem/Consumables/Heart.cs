using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.InventorySystem
{
    [CreateAssetMenu(fileName = "New Heart", menuName = "Inventory System/Item/Consumables/Heart")]
    public class Heart : Consumables
    {
        [SerializeField] Sprite sprite;
        [SerializeField] int amount;
        [SerializeField] string itemName;
        [SerializeField] ItemType itemType;

        void OnEnable()
        {
            Sprite = sprite;
            Amount = amount;
            DisplayName = itemName;
            ItemType = itemType;
        }

        public override void Use(PlayerResources playerResources)
        {
            playerResources.ModifyMaxHealth(amount);
        }
    }
}