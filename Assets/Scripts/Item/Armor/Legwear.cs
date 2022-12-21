using UnityEngine;

[CreateAssetMenu(fileName = "New Armor", menuName = "Inventory System/Item/Armor/Legwear")]
public class Legwear : Armor
{
    [SerializeField] Sprite sprite;
    [SerializeField] string itemName;
    [SerializeField] Rarity rarity;
    [SerializeField] ItemType itemType;

    void OnEnable()
    {
        Sprite = sprite;
        ItemName = itemName;
        Rarity = rarity;
        ItemType = itemType;
    }
}