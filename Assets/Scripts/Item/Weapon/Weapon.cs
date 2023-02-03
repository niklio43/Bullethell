using BulletHell.Abilities;
using BulletHell.Emitters;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : Item
{
    Pool _pool;
    [SerializeField] List<Ability> _abilitySlot = new List<Ability>();
    public AnimatorOverrideController AnimatorController;
    public AbilityDatabase database;
    public Pool Pool { get { return _pool; } set { _pool = value; } }

    public List<Ability> AbilitySlot { get { return _abilitySlot; } set { _abilitySlot = value; } }

    public void Initialize(GameObject owner, GameObject host)
    {
        for (int i = 0; i < _abilitySlot.Count; i++)
        {
            _abilitySlot[i] = database.abilities[_abilitySlot[i].Id];
            _abilitySlot[i] = Instantiate(_abilitySlot[i]);
            _abilitySlot[i].Initialize(owner, host);
        }
    }

    public void Uninitialize()
    {
        for (int i = 0; i < _abilitySlot.Count; i++)
        {
            _abilitySlot[i] = database.abilities[_abilitySlot[i].Id];
            _abilitySlot[i].Uninitialize();
        }
    }

    public void AddAbility(Ability ability, Weapon weapon, GameObject owner)
    {
        if (_abilitySlot.Count >= 4) return;
        if (_abilitySlot.Contains(ability)) { owner.GetComponent<WeaponController>().FillAbilitySlot(weapon); return; }
        _abilitySlot.Add(ability);
        ability.Initialize(owner);
    }
}