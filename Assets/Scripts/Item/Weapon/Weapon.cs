using BulletHell.Abilities;
using BulletHell.Emitters;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public abstract class Weapon : Item
{
    Pool _pool;
    [SerializeField] List<Ability> _abilitySlot = new List<Ability>();
    public AnimatorController animatorController;

    public Pool Pool { get { return _pool; } set { _pool = value; } }

    public List<Ability> AbilitySlot { get { return _abilitySlot; } set { _abilitySlot = value; } }

    public void Initialize(GameObject owner)
    {
        List<Ability> tempAbilities = new List<Ability>();
        foreach (Ability ability in _abilitySlot)
        {
            Ability _ability = Instantiate(ability);
            _ability.Initialize(owner);
            tempAbilities.Add(_ability);
        }
        _abilitySlot = tempAbilities;
    }

    public void Uninitialize()
    {
        foreach (Ability ability in _abilitySlot)
        {
            ability.Uninitialize();
        }
    }

    public void AddAbility(Ability ability, Weapon weapon, GameObject owner)
    {
        if (_abilitySlot.Count >= 4) return;
        if(_abilitySlot.Contains(ability)) { owner.GetComponent<WeaponController>().FillAbilitySlot(weapon); return; }
        _abilitySlot.Add(ability);
        ability.Initialize(owner);
    }
}