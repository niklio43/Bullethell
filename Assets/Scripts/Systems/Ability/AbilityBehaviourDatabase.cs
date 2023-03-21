using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Abilities;

[CreateAssetMenu(fileName = "New Ability Behaviour Database", menuName = "Abilities/BehaviourDatabase")]
public class AbilityBehaviourDatabase : ScriptableObject, ISerializationCallbackReceiver
{
    public BaseAbilityBehaviour[] Behaviours;

    [ContextMenu("Update ID's")]
    public void UpdateID()
    {
        for (int i = 0; i < Behaviours.Length; i++)
        {
            if (Behaviours[i].Id != i)
                Behaviours[i].Id = i;
        }
    }

    public void OnAfterDeserialize()
    {
        UpdateID();
    }

    public void OnBeforeSerialize() { }
}
