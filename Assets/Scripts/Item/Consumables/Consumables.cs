using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.InventorySystem
{
    public class Consumables : InventoryItemData
    {
        int _restoreAmount;

        public int RestoreAmount { get { return _restoreAmount; } set { _restoreAmount = value; } }
    }
}