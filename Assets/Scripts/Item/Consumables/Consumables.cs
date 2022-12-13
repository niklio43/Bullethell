using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumables : Item
{
    int _restoreAmount;

    public int RestoreAmount { get { return _restoreAmount; } set { _restoreAmount = value; } }
}
