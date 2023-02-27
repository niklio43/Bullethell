using UnityEngine;

namespace BulletHell.InventorySystem
{
    [CreateAssetMenu(fileName = "New Armor", menuName = "Inventory System/Item/Armor/Headgear")]
    public class Headgear : Armor
    {
        [SerializeField] Sprite sprite;
        [SerializeField] string itemName;
        [SerializeField] Rarity rarity;
        [SerializeField] ItemType itemType;

        void OnEnable()
        {
            Sprite = sprite;
            DisplayName = itemName;
            Rarity = rarity;
            ItemType = itemType;
        }
    }
}