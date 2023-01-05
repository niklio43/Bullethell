using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Utilities/Stats/PlayerStats")]
public class PlayerStats : CharacterStats
{
    [SerializeField] float health;
    [SerializeField] float maxHealth;
    [SerializeField] float moveSpeed;
    public int DashDistance;

    void OnEnable()
    {
        Health = health;
        MaxHealth = maxHealth;
        MoveSpeed = moveSpeed;
    }

    public void UpdateValues(int value, Attributes attribute)
    {
        for (int i = 0; i < attributes.Length; i++)
        {
            if(attribute == attributes[i].Type)
            {
                attributes[i].Value.ModifiedValue += value;
            }
        }
    }
}