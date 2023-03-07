using UnityEngine;
namespace BulletHell.InventorySystem
{
    public interface IInteractable
    {
        public void Interact(InventorySystem inventory, PlayerResources playerResources);
    }
}