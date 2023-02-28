using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.InventorySystem;
using BulletHell.UI;

public class Forge : InteractableItem
{
    void Awake()
    {
        PlayerUI.Instance.Forge.SetActive(false);
    }

    public override void Interact(InventorySystem inventory)
    {
        PlayerUI.Instance.Forge.SetActive(true);
        PlayerUI.Instance.Inventory.SetActive(true);
    }
}
