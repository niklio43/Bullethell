using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using BulletHell.Stats;
using BulletHell;

[System.Serializable]
public class StatusEffectData
{
    public Status this[string key]
    {
        get
        {
            if (!_status.ContainsKey(key)) return default(Status);
            return _status[key];
        }
    }

    public StatusEffect GetEffect(string key)
    {
        if (!_status.ContainsKey(key)) return null;
        return _status[key].statusEffect;
    }

    [SerializeField] List<Status> _statusList = new List<Status>();

    public Dictionary<string, Status> _status = new Dictionary<string, Status>();

    public void TranslateListToDictionary()
    {
        _status = new Dictionary<string, Status>();

        foreach (Status status in _statusList)
        {
            _status.Add(status.Name, Utilities.Copy(status));
        }
    }

    public void AddModifierToStatus(StatusModifier modifier, float time)
    {
        _status[modifier.Status].AddModifier(modifier, time);
    }

    public void AddModifierToStatus(StatusModifier modifier)
    {
        _status[modifier.Status].AddModifier(modifier);
    }

    public void RemoveModifierFromStatus(StatusModifier modifier)
    {
        _status[modifier.Status].RemoveModifier(modifier);
    }
}
