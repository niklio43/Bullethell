using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.InventorySystem
{
    public class Consumables : InventoryItemData
    {
        int _amount;

        public int Amount { get { return _amount; } set { _amount = value; } }

        public virtual void Use(PlayerResources playerResources) { }
    }
}