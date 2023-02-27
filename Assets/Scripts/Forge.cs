using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.InventorySystem;
using BulletHell.UI;

public class Forge : InteractableItem
{
    void Awake()
    {
        ForgeUI.Instance.gameObject.SetActive(false);
    }

    public override void Interact(InventorySystem inventory)
    {
        ForgeUI.Instance.gameObject.SetActive(true);
        PlayerUI.Instance.Inventory.SetActive(true);
    }
}
