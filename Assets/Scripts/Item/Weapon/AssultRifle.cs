using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Abilities;
using BulletHell.Emitters;

[CreateAssetMenu(fileName = "New Ranged Weapon", menuName = "Inventory System/Item/Weapon/Ranged/AssultRifle")]
public class AssultRifle : Ranged
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