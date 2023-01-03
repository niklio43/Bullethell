using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Emitters;
using BulletHell.Abilities;

public class WeaponController : MonoBehaviour
{
    public Transform CircleOrigin;
    public float Radius;

    /*[SerializeField] Weapon _weapon;
    void Awake()
    {
        AssignWeapon(_weapon);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FillAbilitySlot(_weapon);
        }
    }*/

    public void AssignWeapon(Weapon weapon)
    {
        weapon.Initialize(this);
        GetComponent<SpriteRenderer>().sprite = weapon.Sprite;

        foreach (Ability ability in weapon.Pool._ability)
        {
            GetComponent<AbilityHolder>().abilities.Add(ability);
        }

        var playerSM = transform.GetComponentInParent<PlayerStateMachine>();
        playerSM.Weapon = weapon;

        /*if (GetComponent<Emitter>() == null)
            gameObject.AddComponent<Emitter>();

        var emitter = GetComponent<Emitter>();
        emitter.Data = weapon.EmitterData;
        emitter.Initialize();*/
    }

    public void UnAssignWeapon(Weapon weapon)
    {
        weapon.UnInitialize(this);
        GetComponent<SpriteRenderer>().sprite = null;

        GetComponent<AbilityHolder>().abilities.Clear();

        var playerSM = transform.GetComponentInParent<PlayerStateMachine>();
        playerSM.Weapon = null;
    }

    public void FillAbilitySlot(Weapon weapon)
    {
        weapon.AddAbility(weapon.Pool._ability[Random.Range(0, weapon.Pool._ability.Length)], weapon, this);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Vector3 position = CircleOrigin == null ? Vector3.zero : CircleOrigin.position;
        Gizmos.DrawWireSphere(position, Radius);
    }

    public void DetectColliders()
    {
        foreach (Collider2D collider in Physics2D.OverlapCircleAll(CircleOrigin.position, Radius))
        {
            Debug.Log(collider.name);
        }
    }
}
