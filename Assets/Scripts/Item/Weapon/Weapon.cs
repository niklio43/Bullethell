using BulletHell.Abilities;
using BulletHell.Emitters;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : Item
{
    Pool _pool;
    [SerializeField] List<Ability> _abilitySlot = new List<Ability>();
    public AnimationClip WeaponIdleAnimation;

    public Pool Pool { get { return _pool; } set { _pool = value; } }

    public List<Ability> AbilitySlot { get { return _abilitySlot; } set { _abilitySlot = value; } }

    public void Initialize(GameObject owner)
    {
        foreach(Ability ability in _abilitySlot)
        {
            ability.Initialize(owner);
        }
    }

    public void UnInitialize(GameObject owner)
    {
        foreach (Ability ability in _abilitySlot)
        {
            ability.UnInitialize(owner);
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