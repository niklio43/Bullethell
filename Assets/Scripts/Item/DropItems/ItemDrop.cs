using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemDrop
{
    public Item Item;
    [Range(0f, 1f)] public float Chance;
}
