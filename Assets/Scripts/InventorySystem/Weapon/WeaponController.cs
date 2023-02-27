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

public class WeaponController : MonoBehaviour
{
    public Transform CircleOrigin;
    public float Radius;
    Weapon _weapon;
    [SerializeField] PlayerController _player;

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

    public void UpgradeWeapon(InventoryItemData item)
    {
        if (item == null) { return; }
        Debug.Log("Attempting upgrade");
        FillAbilitySlot(item as Weapon);
    }

    public void FillAbilitySlot(Weapon weapon)
    {
        if (weapon.Abilities.Count >= 3) { Debug.Log("Too many abilities applied!"); FailedUpgrade();  return; }

        if(_player.Character.Stats["Hp"].Get() <= 50) { Debug.Log("Not enough blood!"); FailedUpgrade(); return; }

        Ability ability = weapon.Pool._ability[UnityEngine.Random.Range(0, weapon.Pool._ability.Length)];

        foreach (Ability ab in weapon.Abilities)
        {
            if (ab.Id == ability.Id) { FillAbilitySlot(weapon); return; }
        }
        foreach (Ability ab in weapon.AbilitySlot)
        {
            if (ab.Id == ability.Id) { FillAbilitySlot(weapon); return; }
        }

        StartCoroutine(BeginUpgrade(weapon, ability, _player.gameObject, gameObject));
    }

    IEnumerator BeginUpgrade(Weapon weapon, Ability ability, GameObject owner, GameObject host)
    {
        ForgeUI.Instance.IsUpgrading = true;
        yield return new WaitForSeconds(1f);
        Debug.Log(string.Concat("Added ability: ", ability, " to weapon: ", weapon.DisplayName));
        ForgeUI.Instance.IsUpgrading = false;
        _player.Character.TakeDamage(50);
        weapon.AddAbility(ability, _player.gameObject, gameObject);
    }

    void FailedUpgrade()
    {
        Camera.main.Shake(0.1f, 1f);
    }

    public void Attack(int abilityIndex, InputAction.CallbackContext ctx)
    {
        if (_weapon == null) { return; }
        if (abilityIndex > _weapon.Abilities.Count - 1 || _weapon.Abilities[abilityIndex] == null) { return; }

        _weapon.Abilities[abilityIndex].Cast();
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