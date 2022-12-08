using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Utilities/Stats/PlayerStats")]
public class PlayerStats : CharacterStats
{
    [SerializeField] float health;
    [SerializeField] float maxHealth;
    public float moveSpeed;
    public float dashDistance;

    void OnEnable()
    {
        Health = health;
        MaxHealth = maxHealth;
    }
}