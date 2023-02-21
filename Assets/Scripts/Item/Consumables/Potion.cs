using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.InventorySystem
{
    [CreateAssetMenu(fileName = "New Potion", menuName = "Inventory System/Consumables/Potion")]
    public class Potion : Consumables
    {
        [SerializeField] Sprite sprite;
        [SerializeField] int restoreAmount;
        [SerializeField] string itemName;
        [SerializeField] ItemType itemType;

        void OnEnable()
        {
            Sprite = sprite;
            RestoreAmount = restoreAmount;
            DisplayName = itemName;
            ItemType = itemType;
        }
    }
}