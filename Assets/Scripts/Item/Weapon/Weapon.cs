using BulletHell.Abilities;
using BulletHell.Emitters;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : Item
{
    Pool _pool;
    [SerializeField] List<Ability> _abilitySlot = new List<Ability>();

    public Pool Pool { get { return _pool; } set { _pool = value; } }

    public List<Ability> AbilitySlot { get { return _abilitySlot; } set { _abilitySlot = value; } }

    public void Initialize(WeaponController weaponController)
    {
        foreach(Ability ability in _abilitySlot)
        {
            ability.Initialize(weaponController);
        }
    }

    public void UnInitialize(WeaponController weaponController)
    {
        foreach (Ability ability in _abilitySlot)
        {
            ability.UnInitialize(weaponController);
        }
    }

    public void AddAbility(Ability ability, Weapon weapon, WeaponController weaponController)
    {
        if (_abilitySlot.Count >= 4) return;
        if(_abilitySlot.Contains(ability)) { weaponController.FillAbilitySlot(weapon); return; }
        _abilitySlot.Add(ability);
        ability.Initialize(weaponController);
    }
}