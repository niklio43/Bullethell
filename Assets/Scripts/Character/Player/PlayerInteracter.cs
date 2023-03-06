using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.InventorySystem;

public class PlayerInteracter : MonoBehaviour
{
    [SerializeField, Range(0, 10f)] float _interactRadius = 5f;
    [SerializeField] Material _outlineShader;
    [SerializeField] Material defaultMat;
    InventoryHolder _inventoryHolder;

    LayerMask _interactableMask => 1 << LayerMask.NameToLayer("Interactable");
    IInteractable _closestInteractable = null;
    List<Collider2D> _interactables = new List<Collider2D>();

    void Awake()
    {
        _inventoryHolder = GetComponent<InventoryHolder>();
    }

    void FixedUpdate()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, _interactRadius, _interactableMask);
        if (hit != null)
        {
            InteractableItem interactableItem = hit.GetComponent<InteractableItem>();
            _closestInteractable = interactableItem;

            if (!_interactables.Contains(hit))
            {
                _interactables.Add(hit);
            }

            hit.gameObject.GetComponent<SpriteRenderer>().material = _outlineShader;
        }
        if (!_interactables.Contains(hit))
        {
            _closestInteractable = null;

            for (int i = 0; i < _interactables.Count;)
            {
                if(_interactables[i] != hit)
                {
                    _interactables[i].GetComponent<SpriteRenderer>().material = defaultMat;
                    _interactables.Remove(_interactables[i]);
                    continue;
                }
                i++;
            }
        }
    }

    public void Interact()
    {
        if (_closestInteractable == null) return;
        _closestInteractable.Interact(_inventoryHolder.InventorySystem, GetComponent<PlayerResources>());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _interactRadius);
    }
}