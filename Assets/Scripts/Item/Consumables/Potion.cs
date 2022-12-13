using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Potion", menuName = "Inventory System/Consumables/Potion")]
public class Potion : Consumables
{
    [SerializeField] Sprite sprite;
    [SerializeField] int restoreAmount;
    [SerializeField] string itemName;

    void OnEnable()
    {
        Sprite = sprite;
        RestoreAmount = restoreAmount;
        ItemName = itemName;
    }
}