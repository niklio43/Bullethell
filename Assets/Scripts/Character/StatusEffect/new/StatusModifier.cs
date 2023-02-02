using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatusModifier
{
    public string Status;
    public string Id;

    public StatusModifierType type = StatusModifierType.Add;
    public float Value;

    public enum StatusModifierType
    {
        Add,
        Mult
    }
}
