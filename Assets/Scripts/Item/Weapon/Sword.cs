using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Abilities;

[CreateAssetMenu(fileName = "New Melee Weapon", menuName = "Inventory System/Item/Weapon/Melee/Sword")]
public class Sword : Melee
{
    [SerializeField] Sprite sprite;
    [SerializeField] int damage;
    [SerializeField] string itemName;
    [SerializeField] Pool pool;
    [SerializeField] Rarity rarity;

    void OnEnable()
    {
        Sprite = sprite;
        Damage = damage;
        ItemName = itemName;
        Pool = pool;
        Rarity = rarity;
    }
}