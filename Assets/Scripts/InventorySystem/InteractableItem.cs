using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.InventorySystem;

public class InteractableItem : MonoBehaviour, IPickUp
{
    public virtual void AssignItem() { }

    public virtual void Interact(InventorySystem inventory) { }
}