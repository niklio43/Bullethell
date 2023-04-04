using System.Collections.Generic;
using UnityEngine;
using BulletHell.Abilities;
using System;
using BulletHell.Player;
using BulletHell.InventorySystem;
using BulletHell.GameEventSystem;

public class WeaponController : MonoBehaviour
{
    #region Public Fields
    public Transform CircleOrigin;
    public float Radius;
    public Weapon PrimaryWeapon => _primaryWeapon;
    public Weapon SecondaryWeapon => _secondaryWeapon;
    #endregion

    #region Private Fields
    Weapon _equippedWeapon;
    Weapon _primaryWeapon;
    Weapon _secondaryWeapon;
    [SerializeField] PlayerController _player;
    #endregion

    #region Public Methods

    public void AssignPrimaryWeapon(Component sender, object data)
    {
        UnAssignWeapon(_primaryWeapon);
        _primaryWeapon = null;
        EquippedWeaponUI.Instance.SetWeapon(null, false, true);

        if (data is not InventoryItemData) { return; }
        var item = data as InventoryItemData;
        if(item is not Weapon) { return; }

        _primaryWeapon = item as Weapon;
        EquippedWeaponUI.Instance.SetWeapon(_primaryWeapon.Icon, true, true);
        EquippedWeaponUI.Instance.SetActiveWeapon(true);
        AssignWeapon(_primaryWeapon);
    }

    public void AssignSecondaryWeapon(Component sender, object data)
    {
        UnAssignWeapon(_secondaryWeapon);
        _secondaryWeapon = null;
        EquippedWeaponUI.Instance.SetWeapon(null, false, false);

        if (data is not InventoryItemData) { return; }
        var item = data as InventoryItemData;
        if (item is not Weapon) { return; }

        _secondaryWeapon = item as Weapon;
        EquippedWeaponUI.Instance.SetWeapon(_secondaryWeapon.Icon, true, false);
    }

    public void SetActiveWeapon(bool activeWeapon)
    {
        if (activeWeapon)
        {
            if (_primaryWeapon == null) { return; }
            EquippedWeaponUI.Instance.SetActiveWeapon(true);
            AssignWeapon(_primaryWeapon);
        }
        else
        {
            if (_secondaryWeapon == null) { return; }
            EquippedWeaponUI.Instance.SetActiveWeapon(false);
            AssignWeapon(_secondaryWeapon);
        }
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
    #endregion

    #region Private Methods
    void AssignWeapon(Weapon weapon)
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

    void UnAssignWeapon(Weapon weapon)
    {
        if (weapon == null) { return; }
        GetComponent<SpriteRenderer>().sprite = null;

        GetComponent<AbilityHolder>().SetBaseAbility(null);

        GetComponent<AbilityHolder>().AddAbility(null);

        GetComponent<Animator>().runtimeAnimatorController = null;
        weapon.Uninitialize();
        _equippedWeapon = null;
    }
    #endregion

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