using UnityEngine;
namespace BulletHell.InventorySystem
{
    internal interface IInteractable
    {
        public void Interact(InventorySystem inventory, PlayerResources playerResources);
    }
}