using UnityEngine;

[CreateAssetMenu(fileName = "New Armor", menuName = "Inventory System/Item/Armor/Chestwear")]
public class Chestwear : Armor
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
        ItemName = ItemName;
        Rarity = rarity;
        ItemType = itemType;
    }
}