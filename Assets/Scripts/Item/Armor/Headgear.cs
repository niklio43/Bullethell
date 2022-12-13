using UnityEngine;

[CreateAssetMenu(fileName = "New Armor", menuName = "Inventory System/Item/Armor/Headgear")]
public class Headgear : Armor
{
    [SerializeField] int defense;
    [SerializeField] Sprite sprite;
    [SerializeField] string itemName;
    [SerializeField] Rarity rarity;

    void OnEnable()
    {
        Defense = defense;
        Sprite = sprite;
        ItemName = itemName;
        Rarity = rarity;
    }
}