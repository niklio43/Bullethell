using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Emitters;

[CreateAssetMenu(fileName = "New Ranged Weapon", menuName = "Inventory System/Item/Weapon/Ranged/AssultRifle")]
public class AssultRifle : Ranged
{
    [SerializeField] Sprite sprite;
    [SerializeField] int damage;
    [SerializeField] string itemName;
    [SerializeField] EmitterData emitterData;
    [SerializeField] Rarity rarity;
    [SerializeField] ItemType itemType;

    void OnEnable()
    {
        Sprite = sprite;
        Damage = damage;
        ItemName = itemName;
        EmitterData = emitterData;
        Rarity = rarity;
        ItemType = itemType;
    }
}