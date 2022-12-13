using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteracter : MonoBehaviour
{
    [SerializeField, Range(0, 100f)] float _interactRadius = 5f;
    [SerializeField] LayerMask _interactable;
    [SerializeField] Inventory _inventory;
    IInteractable _closestInteractable = null;

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
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            _inventory.Load();
        }
    }

    public void Interact()
    {
        if (_closestInteractable == null) return;
        _closestInteractable.Interact(_inventory);
    }

    void OnApplicationQuit()
    {
        _inventory.Container.Items.Clear();
    }
}
