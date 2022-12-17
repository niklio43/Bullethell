using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Utilities/Stats/PlayerStats")]
public class PlayerStats : CharacterStats
{
    public float moveSpeed;
    public float dashDistance;
    public int stamina;
    int baseDef;
    int baseAtk;
    int baseStamina;

    void OnEnable()
    {
        baseDef = Defense;
        baseAtk = Attack;
        baseStamina = stamina;
    }

    public void UpdateValues(int value, Attributes attribute)
    {
        if(attribute == Attributes.Defense)
            Defense = baseDef + value;

        if (attribute == Attributes.Attack)
            Attack = baseAtk + value;

        if (attribute == Attributes.Stamina)
            stamina = baseStamina + value;
    }
}