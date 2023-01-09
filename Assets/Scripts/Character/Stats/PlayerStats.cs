using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Utilities/Stats/PlayerStats")]
public class PlayerStats : CharacterStats
{
    public int DashDistance;

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