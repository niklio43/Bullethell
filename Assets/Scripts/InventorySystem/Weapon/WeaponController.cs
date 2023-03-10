using System.Collections.Generic;
using UnityEngine;
using BulletHell.Abilities;
using System;
using BulletHell.Player;
using BulletHell.InventorySystem;

public class WeaponController : MonoBehaviour
{
    public Transform CircleOrigin;
    public float Radius;
    Weapon _equippedWeapon;
    Weapon _primaryWeapon;
    Weapon _secondaryWeapon;
    [SerializeField] PlayerController _player;
    [SerializeField] InventorySlotUI _primaryWeaponSlot;
    [SerializeField] InventorySlotUI _secondaryWeaponSlot;

    public Weapon PrimaryWeapon => _primaryWeapon;
    public Weapon SecondaryWeapon => _secondaryWeapon;

    private void Start()
    {
        _primaryWeaponSlot.AssignedInventorySlot.OnAssign += AssignPrimaryWeapon;
        _secondaryWeaponSlot.AssignedInventorySlot.OnAssign += AssignSecondaryWeapon;
    }

    public void AssignPrimaryWeapon(InventoryItemData item)
    {
        if (item == null)
        {
            UnAssignWeapon(_primaryWeapon);
            _primaryWeapon = null;
            EquippedWeaponUI.Instance.SetWeapon
                (EquippedWeaponUI.Instance.PrimaryWeapon, null, false);
            return;
        }
        _primaryWeapon = item as Weapon;
        EquippedWeaponUI.Instance.SetWeapon
            (EquippedWeaponUI.Instance.PrimaryWeapon, _primaryWeapon.Icon, true);
        EquippedWeaponUI.Instance.SetActiveWeapon
            (EquippedWeaponUI.Instance.PrimaryWeapon, EquippedWeaponUI.Instance.SecondaryWeapon);
        AssignWeapon(_primaryWeapon);
    }

    public void AssignSecondaryWeapon(InventoryItemData item)
    {
        if (item == null)
        {
            UnAssignWeapon(_secondaryWeapon);
            _secondaryWeapon = null;
            EquippedWeaponUI.Instance.SetWeapon
                (EquippedWeaponUI.Instance.SecondaryWeapon, null, false);
            return;
        }
        _secondaryWeapon = item as Weapon;
        EquippedWeaponUI.Instance.SetWeapon
            (EquippedWeaponUI.Instance.SecondaryWeapon, _secondaryWeapon.Icon, true);
    }

    public void SetActiveWeapon(bool activeWeapon)
    {
        if (activeWeapon)
        {
            if (_primaryWeapon == null) { return; }
            EquippedWeaponUI.Instance.SetActiveWeapon
                (EquippedWeaponUI.Instance.PrimaryWeapon, EquippedWeaponUI.Instance.SecondaryWeapon);
            AssignWeapon(_primaryWeapon);
        }
        else
        {
            if (_secondaryWeapon == null) { return; }
            EquippedWeaponUI.Instance.SetActiveWeapon
                (EquippedWeaponUI.Instance.SecondaryWeapon, EquippedWeaponUI.Instance.PrimaryWeapon);
            AssignWeapon(_secondaryWeapon);
        }
    }

    public void AssignWeapon(Weapon weapon)
    {
        UnAssignWeapon(weapon);
        weapon.Initialize(_player.gameObject, gameObject);

        Animator animator = GetComponent<Animator>();

        animator.runtimeAnimatorController = weapon.AnimatorController;

        GetComponent<Animator>().Play("Idle");
        GetComponent<SpriteRenderer>().sprite = weapon.Sprite;
        gameObject.name = weapon.name;

        GetComponent<AbilityHolder>().SetBaseAbility(weapon.BaseAbility);

        foreach (Ability ability in weapon.Abilities)
        {
            GetComponent<AbilityHolder>().AddAbility(ability);
        }
        _equippedWeapon = weapon;
    }

    public void UnAssignWeapon(Weapon weapon)
    {
        if (weapon == null) { return; }
        GetComponent<SpriteRenderer>().sprite = null;

        GetComponent<AbilityHolder>().SetBaseAbility(null);

        GetComponent<AbilityHolder>().AddAbility(null);

        GetComponent<Animator>().runtimeAnimatorController = null;
        weapon.Uninitialize();
        _equippedWeapon = null;
    }

    public void Attack()
    {
        if (_equippedWeapon == null) { return; }
        _equippedWeapon.BaseAbility.Cast(_player.PlayerMovement.MousePosition);
    }

    public void UseAbility(int abilityIndex)
    {
        if (_equippedWeapon == null) { return; }
        if (abilityIndex > _equippedWeapon.Abilities.Count - 1 || _equippedWeapon.Abilities[abilityIndex] == null) { return; }

        _equippedWeapon.Abilities[abilityIndex].Cast(_player.PlayerMovement.MousePosition);
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