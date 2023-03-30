using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.InventorySystem
{
    public class Consumables : InventoryItemData
    {
        #region Private Fields
        int _amount;
        #endregion

        #region Public Fields
        public int Amount { get { return _amount; } set { _amount = value; } }
        #endregion

        #region Public Methods
        public virtual void Use(PlayerResources playerResources) { }
        #endregion
    }
}