using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Status
{
    public string Name;
    public StatusEffect statusEffect;
    Dictionary<string, StatusModifier> _statusModifiers = new Dictionary<string, StatusModifier>();

    public void AddModifier(StatusModifier statusModifier, float time)
    {
        if (_statusModifiers.ContainsKey(statusModifier.Id))
        {
            Debug.LogWarning("A modifier with the same id is already applied to the stat.");
            return;
        }

        AddModifier(statusModifier);
        //MonoInstance.GetInstance().Invoke(() => { RemoveModifier(statusModifier); }, time);
    }

    public void AddModifier(StatusModifier statusModifier)
    {
        if (_statusModifiers.ContainsKey(statusModifier.Id))
        {
            Debug.LogWarning("A modifier with the same id is already applied to the stat.");
            return;
        }

        _statusModifiers.Add(statusModifier.Id, statusModifier);
    }

    public void RemoveModifier(StatusModifier statusModifier)
    {
        _statusModifiers.Remove(statusModifier.Id);
    }

}