using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteracter : MonoBehaviour
{
    [SerializeField, Range(0, 100f)] float _interactRadius = 5f;
    [SerializeField] LayerMask _interactable;
    IInteractable _closestInteractable = null;
    SlotListener _slotListener;

    void FixedUpdate()
    {
        _slotListener = GetComponentInChildren<SlotListener>();
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
            _slotListener.Inventory.Save();
            _slotListener.Equipment.Save();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            _slotListener.Inventory.Load();
            _slotListener.Equipment.Load();
        }
    }

    public void Interact()
    {
        if (_closestInteractable == null) return;
        _closestInteractable.Interact(_slotListener.Inventory);
    }

    void OnApplicationQuit()
    {
        _slotListener.Inventory.Clear();
        _slotListener.Equipment.Clear();
    }
}