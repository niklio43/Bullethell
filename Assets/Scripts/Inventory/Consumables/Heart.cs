using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.InventorySystem
{
    [CreateAssetMenu(fileName = "New Heart", menuName = "Inventory System/Item/Consumables/Heart")]
    public class Heart : Consumables
    {
        #region Private Fields
        [SerializeField] Sprite sprite;
        [SerializeField] int amount;
        [SerializeField] string itemName;
        [SerializeField] ItemType itemType;
        #endregion

        #region Private Methods
        void OnEnable()
        {
            Sprite = sprite;
            Amount = amount;
            DisplayName = itemName;
            ItemType = itemType;
        }
        #endregion

        #region Public Methods
        public override void Use(PlayerResources playerResources)
        {
            playerResources.ModifyMaxHealth(amount);
        }
        #endregion
    }
}