using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ranged Weapon", menuName = "Weapon/Ranged/AssultRifle")]
public class AssultRifle : Ranged
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