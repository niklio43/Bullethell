using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.InventorySystem;

public class Forge : InteractableItem
{
    [SerializeField] GameObject _forgeUI;

    void Awake()
    {
        _forgeUI.SetActive(false);
    }

    public override void Interact(InventorySystem inventory)
    {
        _forgeUI.SetActive(true);
    }
}
