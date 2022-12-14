using UnityEngine;

[CreateAssetMenu(fileName = "New Armor", menuName = "Inventory System/Item/Armor/Legwear")]
public class Legwear : Armor
{
    [SerializeField] int defense;
    [SerializeField] Sprite sprite;
    [SerializeField] string itemName;
    [SerializeField] Rarity rarity;
    [SerializeField] ItemType itemType;

    void OnEnable()
    {
        Defense = defense;
        Sprite = sprite;
        ItemName = itemName;
        Rarity = rarity;
        ItemType = itemType;
    }
}