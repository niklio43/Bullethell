using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteracter : MonoBehaviour
{
    [SerializeField, Range(0, 100f)] float _interactRadius = 5f;
    [SerializeField] LayerMask _interactable;
    [SerializeField] Inventory _inventory;
    [SerializeField] Inventory _equipment;
    IInteractable _closestInteractable = null;

    void Start()
    {
        for (int i = 0; i < _equipment.GetSlots.Length; i++)
        {
            _equipment.GetSlots[i].OnBeforeUpdate += OnBeforeSlotUpdate;
            _equipment.GetSlots[i].OnAfterUpdate += OnAfterSlotUpdate;
        }
    }

    public void OnBeforeSlotUpdate(InventorySlot slot)
    {
        if (slot.Item == null) return;

        switch (slot.Parent.Inventory.type)
        {
            case InterfaceType.Inventory:
                break;
            case InterfaceType.Equipment:
                print(string.Concat("Removed ", slot.Item.Name, " on ", slot.Parent.Inventory.type, ", Allowed Items: ", string.Join(", ", slot.AllowedItems)));
                //remove stats
                break;
            case InterfaceType.Dialogue:
                break;
            default:
                break;
        }
    }

    public void OnAfterSlotUpdate(InventorySlot slot)
    {
        switch (slot.Parent.Inventory.type)
        {
            case InterfaceType.Inventory:
                break;
            case InterfaceType.Equipment:
                print(string.Concat("Placed ", slot.Item.Name, " on ", slot.Parent.Inventory.type, ", Allowed Items: ", string.Join(", ", slot.AllowedItems)));
                //add stats
                break;
            case InterfaceType.Dialogue:
                break;
            default:
                break;
        }
    }

    void FixedUpdate()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, _interactRadius, _interactable);
        if (hit != null)
        {
            DroppedItem droppedItem = hit.GetComponent<DroppedItem>();
            _closestInteractable = droppedItem;
        }
        else
        {
            _closestInteractable = null;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            _inventory.Save();
            _equipment.Save();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            _inventory.Load();
            _equipment.Load();
        }
    }

    public void Interact()
    {
        if (_closestInteractable == null) return;
        _closestInteractable.Interact(_inventory);
    }

    void OnApplicationQuit()
    {
        _inventory.Clear();
        _equipment.Clear();
    }
}
