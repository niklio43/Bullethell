using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Emitters;
using BulletHell.Abilities;
using UnityEngine.VFX;
using System;
using BulletHell.Player;
using UnityEngine.InputSystem;
using BulletHell.InventorySystem;
using BulletHell.CameraUtilities;
using BulletHell.UI;

public class WeaponController : MonoBehaviour
{
    public Transform CircleOrigin;
    public float Radius;
    Weapon _weapon;
    [SerializeField] PlayerController _player;
    [SerializeField] List<InventorySlotUI> _slots = new List<InventorySlotUI>();

    private void Start()
    {
        foreach(InventorySlotUI slot in _slots)
        {
            slot.AssignedInventorySlot.OnAssign += EquipWeapon;
        }
    }

    public void EquipWeapon(InventoryItemData item)
    {
        if (item == null)
        {
            UnAssignWeapon();
            return;
        }
        AssignWeapon(item as Weapon);
    }

    public void AssignWeapon(Weapon weapon)
    {
        weapon.Initialize(_player.gameObject, gameObject);

        Animator animator = GetComponent<Animator>();

        animator.runtimeAnimatorController = weapon.AnimatorController;

        GetComponent<Animator>().Play("Idle");
        GetComponent<SpriteRenderer>().sprite = weapon.Sprite;
        gameObject.name = weapon.name;

        foreach (Ability ability in weapon.Abilities)
        {
            GetComponent<AbilityHolder>().AddAbility(ability);
        }
        _weapon = weapon;
    }

    public void UnAssignWeapon()
    {
        GetComponent<SpriteRenderer>().sprite = null;

        GetComponent<AbilityHolder>().AddAbility(null);

        GetComponent<Animator>().runtimeAnimatorController = null;
        _weapon.Uninitialize();
        _weapon = null;
    }

    public void Attack(int abilityIndex, InputAction.CallbackContext ctx)
    {
        if (_weapon == null) { return; }
        if (abilityIndex > _weapon.Abilities.Count - 1 || _weapon.Abilities[abilityIndex] == null) { return; }

        //FIX SO THAT CURSOR IS THE TARGET
        _weapon.Abilities[abilityIndex].Cast(Vector3.zero);
    }

    #region Component Caching

    Dictionary<Type, Component> _cachedComponents = new Dictionary<Type, Component>();
    public new T GetComponent<T>() where T : Component
    {
        if (_cachedComponents.ContainsKey(typeof(T)))
        {
            return (T)_cachedComponents[typeof(T)];
        }

        var component = base.GetComponent<T>();
        if (component != null)
        {
            _cachedComponents.Add(typeof(T), component);
        }
        return component;
    }

    #endregion
}