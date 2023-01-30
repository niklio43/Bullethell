using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Emitters;
using BulletHell.Abilities;
using UnityEngine.VFX;
using System;
using BulletHell.Player;
using UnityEngine.InputSystem;

public class WeaponController : MonoBehaviour
{
    public Transform CircleOrigin;
    public float Radius;
    Weapon _weapon;
    [SerializeField] PlayerController player;

    public void AssignWeapon(Weapon weapon)
    {
        Debug.Log("Assign Weapon");
        weapon.Initialize(player.gameObject, gameObject);

        Animator animator = GetComponent<Animator>();

        animator.runtimeAnimatorController = weapon.AnimatorController;

        GetComponent<Animator>().Play("Idle");
        GetComponent<SpriteRenderer>().sprite = weapon.Sprite;
        gameObject.name = weapon.name;

        foreach (Ability ability in weapon.AbilitySlot)
        {
            GetComponent<AbilityHolder>().AddAbility(ability);
        }
        _weapon = weapon;
    }

    public void UnAssignWeapon(Weapon weapon)
    {
        Debug.Log("testerobama");
        weapon.Uninitialize();
        GetComponent<SpriteRenderer>().sprite = null;

        GetComponent<AbilityHolder>().AddAbility(null);

        GetComponent<Animator>().runtimeAnimatorController = null;
        _weapon = null;
    }

    //TODO Add additional functionality.
    public void FillAbilitySlot(Weapon weapon)
    {
        weapon.AddAbility(weapon.Pool._ability[UnityEngine.Random.Range(0, weapon.Pool._ability.Length)], weapon, gameObject);
    }

    public void Attack(int abilityIndex, InputAction.CallbackContext ctx)
    {
        if (_weapon == null) { Debug.Log("No Weapon Error"); return; }
        if (abilityIndex > _weapon.AbilitySlot.Count - 1 || _weapon.AbilitySlot[abilityIndex] == null) { Debug.Log("Ability doesn't exist!"); return; }

        _weapon.AbilitySlot[abilityIndex].Cast();
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