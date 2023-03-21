using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.InventorySystem;

namespace BulletHell.Player
{
    public class PlayerInteracter : MonoBehaviour
    {
        [SerializeField, Range(0, 100f)] float _interactRadius = 5f;
        [SerializeField] Material _outlineShader;
        [SerializeField] Material _defaultMaterial;
        [SerializeField, Range(0, 10)] float _drawInObjectSpeed = 1f;
        InventoryHolder _inventoryHolder;
        [SerializeField] InventoryHolder _equipmentInventory;
        LayerMask _interactableMask => 1 << LayerMask.NameToLayer("Interactable");
        IInteractable _closestInteractable = null;

        public IInteractable ClosestInteractable { get { return _closestInteractable; } set { _closestInteractable = value; } }

        void Awake()
        {
            _inventoryHolder = GetComponent<InventoryHolder>();
        }

        void FixedUpdate()
        {
            Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, _interactRadius, _interactableMask);

            if (_closestInteractable != null)
            {
                InteractableItem item = (InteractableItem)_closestInteractable;
                item.GetComponent<SpriteRenderer>().material = _defaultMaterial;
                _closestInteractable = null;
            }

            if (hit == null) { return; }

            if (CheckForConsumable(hit)) { return; }

            Transform closestTransform = GetClosest(hit);

            if (closestTransform == null) { return; }

            if (!closestTransform.TryGetComponent(out InteractableItem closest)) { return; }

            _closestInteractable = closest;

            closest.gameObject.GetComponent<SpriteRenderer>().material = _outlineShader;
        }

        bool CheckForConsumable(Collider2D[] hit)
        {
            for (int i = 0; i < hit.Length; i++)
            {
                InteractableItem item = hit[i].GetComponent<InteractableItem>();
                if (item is DroppedItem)
                {
                    var droppedItem = item as DroppedItem;
                    if (droppedItem.ItemData.ItemType == ItemType.Consumable && droppedItem != null)
                    {
                        DrawInObject(item.transform);
                        return true;
                    }
                }
            }
            return false;
        }

        Transform GetClosest(Collider2D[] item)
        {
            Transform bestTarget = null;
            float closestDistanceSqr = Mathf.Infinity;
            Vector3 currentPosition = transform.position;
            foreach (Collider2D potentialTarget in item)
            {
                Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if (dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    bestTarget = potentialTarget.transform;
                }
            }
            return bestTarget;
        }

        void DrawInObject(Transform obj)
        {
            float distance = Vector2.Distance(obj.position, transform.position);
            Vector2 direction = transform.position - obj.position;
            obj.position += (Vector3)direction * Time.deltaTime * _drawInObjectSpeed;

            if (distance > 0.5f) { return; }
            obj.GetComponent<DroppedItem>().Use(GetComponent<PlayerResources>());
        }

        public void Interact()
        {
            if (_closestInteractable == null) return;
            if (CheckForFreeEquipmentSlot()) { return; }
            _closestInteractable.Interact(_inventoryHolder.InventorySystem, GetComponent<PlayerResources>());
            _closestInteractable = null;
        }

        public bool CheckForFreeEquipmentSlot()
        {
            var droppedItem = _closestInteractable.GetComponent<DroppedItem>();
            if (droppedItem != null && droppedItem.ItemData.ItemType == ItemType.Weapon)
            {
                WeaponController weaponController = GetComponent<PlayerController>().WeaponController;

                if (weaponController.PrimaryWeapon == null)
                {
                    weaponController.AssignPrimaryWeapon(droppedItem.ItemData);
                    _closestInteractable.Interact(_equipmentInventory.InventorySystem, GetComponent<PlayerResources>());
                    _closestInteractable = null;
                    return true;
                }
                if (weaponController.SecondaryWeapon == null)
                {
                    weaponController.AssignSecondaryWeapon(droppedItem.ItemData);
                    _closestInteractable.Interact(_equipmentInventory.InventorySystem, GetComponent<PlayerResources>());
                    _closestInteractable = null;
                    return true;
                }
                return false;
            }
            return false;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _interactRadius);
        }
    }
}