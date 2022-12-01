using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Melee Weapon", menuName = "Weapon/Melee/Sword")]
public class Sword : Melee
{
    [SerializeField] Sprite sprite;
    [SerializeField] int damage;
    [SerializeField] string itemName;

    void OnEnable()
    {
        Sprite = sprite;
        Damage = damage;
        ItemName = itemName;
    }
}