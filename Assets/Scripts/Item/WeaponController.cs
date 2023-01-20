using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Emitters;
using BulletHell.Abilities;
using UnityEngine.VFX;
using System;
using BulletHell.Player;

public class WeaponController : MonoBehaviour
{
    public Transform CircleOrigin;
    public float Radius;

    public void AssignWeapon(Weapon weapon)
    {
        weapon.Initialize(gameObject);
        GetComponent<Animator>().runtimeAnimatorController = weapon.animatorController;
        GetComponent<Animator>().Play("Idle");
        GetComponent<SpriteRenderer>().sprite = weapon.Sprite;
        gameObject.name = weapon.name;

        foreach (Ability ability in weapon.AbilitySlot)
        {
            GetComponent<AbilityHolder>().AddAbility(ability);
        }

        var playerSM = transform.GetComponentInParent<PlayerController>();
        playerSM.Weapon = weapon;
    }

    public void UnAssignWeapon(Weapon weapon)
    {
        weapon.Uninitialize();
        GetComponent<SpriteRenderer>().sprite = null;

        GetComponent<AbilityHolder>().AddAbility(null);

        GetComponent<Animator>().runtimeAnimatorController = null;

        var playerSM = transform.GetComponentInParent<PlayerController>();
        playerSM.Weapon = null;
    }

    //TODO Add additional functionality.
    public void FillAbilitySlot(Weapon weapon)
    {
        weapon.AddAbility(weapon.Pool._ability[UnityEngine.Random.Range(0, weapon.Pool._ability.Length)], weapon, gameObject);
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