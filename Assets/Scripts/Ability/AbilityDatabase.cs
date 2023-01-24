using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Abilities;

[CreateAssetMenu(fileName = "New Ability Database", menuName = "Abilities/Database")]
public class AbilityDatabase : ScriptableObject, ISerializationCallbackReceiver
{
    public Ability[] abilities;

    [ContextMenu("Update ID's")]
    public void UpdateID()
    {
        for (int i = 0; i < abilities.Length; i++)
        {
            if (abilities[i].Id != i)
                abilities[i].Id = i;
        }
    }

    public void OnAfterDeserialize()
    {
        UpdateID();
    }

    public void OnBeforeSerialize() { }
}
