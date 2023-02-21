using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.InventorySystem;

public class PlayerInteracter : MonoBehaviour
{
    [SerializeField, Range(0, 10f)] float _interactRadius = 5f;
    InventoryHolder _inventoryHolder;

    LayerMask _interactableMask => 1 << LayerMask.NameToLayer("Interactable");
    IInteractable _closestInteractable = null;

    void Awake()
    {
        _inventoryHolder = GetComponent<InventoryHolder>();
    }

    void FixedUpdate()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, _interactRadius, _interactableMask);
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

    public void Interact()
    {
        if (_closestInteractable == null) return;
        _closestInteractable.Interact(_inventoryHolder.InventorySystem);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _interactRadius);
    }
}