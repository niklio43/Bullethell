using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "Utilities/Stats/EnemyStats")]
public class EnemyStats : CharacterStats
{
    [SerializeField] float health;
    [SerializeField] float maxHealth;

    void OnEnable()
    {
        Health = health;
        MaxHealth = maxHealth;
    }
}