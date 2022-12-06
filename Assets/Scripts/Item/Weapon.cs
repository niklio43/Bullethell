using BulletHell.Abilities;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : Item
{
    int _damage;
    Pool _pool;
    [SerializeField] List<Ability> _abilitySlot = new List<Ability>();

    public int Damage { get { return _damage; } set { _damage = value; } }
    public Pool Pool { get { return _pool; } set { _pool = value; } }
    public List<Ability> AbilitySlot { get { return _abilitySlot; } set { _abilitySlot = value; } }

    public void AddAbility(Ability ability, Weapon weapon, WeaponController weaponController)
    {
        if (_abilitySlot.Count >= 3) return;
        if(_abilitySlot.Contains(ability)) { weaponController.FillAbilitySlot(weapon); return; }
        _abilitySlot.Add(ability);
    }
}