using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Abilities;

public class AbilityHolder : MonoBehaviour
{
    [HideInInspector] public List<Ability> abilities;

    void Update()
    {
        for (int i = 0; i < abilities.Count; i++)
        {
            abilities[i].UpdateAbility(Time.deltaTime);
        }
    }
}
