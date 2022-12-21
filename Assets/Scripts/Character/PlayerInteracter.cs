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

    [SerializeField] PlayerStats _stats;

    void Start()
    {
        for (int i = 0; i < _stats.attributes.Length; i++)
        {
            _stats.attributes[i].SetParent(this);
        }

        for (int i = 0; i < _equipment.GetSlots.Length; i++)
        {
            _equipment.GetSlots[i].OnBeforeUpdate += OnBeforeSlotUpdate;
            _equipment.GetSlots[i].OnAfterUpdate += OnAfterSlotUpdate;
        }
    }

    public void OnBeforeSlotUpdate(InventorySlot slot)
    {
        if (slot.Item.Id == -1) return;

        switch (slot.Parent.Inventory.type)
        {
            case InterfaceType.Inventory:
                break;
            case InterfaceType.Equipment:
                print(string.Concat("Removed ", slot.Item.Name, " on ", slot.Parent.Inventory.type, ", Allowed Items: ", string.Join(", ", slot.AllowedItems)));
                for (int i = 0; i < slot.Item.buffs.Length; i++)
                {
                    for (int j = 0; j < _stats.attributes.Length; j++)
                    {
                        if (_stats.attributes[j].Type == slot.Item.buffs[i].attribute)
                        {
                            _stats.attributes[j].Value.RemoveModifier(slot.Item.buffs[i]);
                        }
                    }
                }
                break;
            case InterfaceType.Dialogue:
                break;
            default:
                break;
        }
    }

    public void OnAfterSlotUpdate(InventorySlot slot)
    {
        if (slot.Item.Id == -1) return;

        switch (slot.Parent.Inventory.type)
        {
            case InterfaceType.Inventory:
                break;
            case InterfaceType.Equipment:
                print(string.Concat("Placed ", slot.Item.Name, " on ", slot.Parent.Inventory.type, ", Allowed Items: ", string.Join(", ", slot.AllowedItems)));
                for (int i = 0; i < slot.Item.buffs.Length; i++)
                {
                    for (int j = 0; j < _stats.attributes.Length; j++)
                    {
                        if (_stats.attributes[j].Type == slot.Item.buffs[i].attribute)
                        {
                            _stats.attributes[j].Value.AddModifier(slot.Item.buffs[i]);
                        }
                    }
                }
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

    public void AttributeModified(Attribute attribute)
    {
        Debug.Log(string.Concat(attribute.Type, " was updated! Value is now ", attribute.Value.ModifiedValue));
        _stats.UpdateValues(attribute.Value.ModifiedValue, attribute.Type);
    }

    void OnApplicationQuit()
    {
        _inventory.Clear();
        _equipment.Clear();
    }
}

[System.Serializable]
public class Attribute
{
    [System.NonSerialized]
    public PlayerInteracter Parent;
    public Attributes Type;
    public ModifiableInt Value;

    public void SetParent(PlayerInteracter parent)
    {
        Parent = parent;
        Value = new ModifiableInt(AttributeModified);
    }
    public void AttributeModified()
    {
        Parent.AttributeModified(this);
    }
}