using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Emitters;
using BulletHell.Abilities;
using UnityEngine.VFX;
using System;

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
            GetComponent<AbilityHolder>().abilities.Add(ability);
        }

        var playerSM = transform.GetComponentInParent<PlayerStateMachine>();
        playerSM.Weapon = weapon;
    }

    public void UnAssignWeapon(Weapon weapon)
    {
        weapon.Uninitialize();
        GetComponent<SpriteRenderer>().sprite = null;

        GetComponent<AbilityHolder>().abilities.Clear();

        GetComponent<Animator>().runtimeAnimatorController = null;

        var playerSM = transform.GetComponentInParent<PlayerStateMachine>();
        playerSM.Weapon = null;
    }

    //TODO Add additional functionality.
    public void FillAbilitySlot(Weapon weapon)
    {
        weapon.AddAbility(weapon.Pool._ability[UnityEngine.Random.Range(0, weapon.Pool._ability.Length)], weapon, gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Vector3 position = CircleOrigin == null ? Vector3.zero : CircleOrigin.position;
        Gizmos.DrawWireSphere(position, Radius);
    }

    //TODO Add additional functionality.
    public void DetectColliders(float damage)
    {
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(CircleOrigin.position, Radius))
        {
            Debug.Log(collider.name);
            GameObject other = collider.gameObject;
            if (other.CompareTag("Enemy"))
            {
                other.GetComponent<Enemy>().TakeDamage(damage);
            }
        }
    }

    public void PlayVfx(VisualEffectAsset vfx)
    {
        var visualEffect = GetComponent<VisualEffect>();
        visualEffect.visualEffectAsset = vfx;
        visualEffect.Play();

        visualEffect.SetVector3("angle", transform.parent.eulerAngles);
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