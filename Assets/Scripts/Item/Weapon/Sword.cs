using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Abilities;

[CreateAssetMenu(fileName = "New Melee Weapon", menuName = "Inventory System/Item/Weapon/Melee/Sword")]
public class Sword : Melee
{
    [SerializeField] Sprite sprite;
    [SerializeField] string itemName;
    [SerializeField] Pool pool;
    [SerializeField] Rarity rarity;
    [SerializeField] ItemType itemType;

    void OnEnable()
    {
        Sprite = sprite;
        ItemName = itemName;
        Pool = pool;
        Rarity = rarity;
        ItemType = itemType;
    }
}