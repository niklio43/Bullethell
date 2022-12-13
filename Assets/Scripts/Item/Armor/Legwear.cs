using UnityEngine;

[CreateAssetMenu(fileName = "New Armor", menuName = "Inventory System/Item/Armor/Legwear")]
public class Legwear : Armor
{
    [SerializeField] int defense;
    [SerializeField] Sprite sprite;
    [SerializeField] string itemName;

    void OnEnable()
    {
        Defense = defense;
        Sprite = sprite;
        ItemName = itemName;
    }
}