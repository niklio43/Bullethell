using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Emitters;

public class WeaponController : MonoBehaviour
{
    [SerializeField] Weapon _weapon;
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
    }

    public void AssignWeapon(Weapon weapon)
    {
        weapon.Initialize(this);
        GetComponent<SpriteRenderer>().sprite = weapon.Sprite;
        if (weapon.GetType().Equals(typeof(Ranged)))
        {
            Ranged rangedWeapon = (Ranged)weapon;
            AssignWeapon(rangedWeapon);
            return;
        }
        Melee meleeWeapon = (Melee)weapon;
        AssignWeapon(meleeWeapon);
    }

    public void AssignWeapon(Ranged weapon)
    {
        var emitter = GetComponent<Emitter>();
        emitter.Data = weapon.EmitterData;
        emitter.Initialize();
    }

    public void AssignWeapon(Melee weapon) { }

    public void FillAbilitySlot(Weapon weapon)
    {
        weapon.AddAbility(weapon.Pool._ability[Random.Range(0, weapon.Pool._ability.Length)], weapon, this);
    }
}
