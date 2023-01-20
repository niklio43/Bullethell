using BulletHell.Abilities;
using BulletHell.Emitters;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public abstract class Weapon : Item
{
    Pool _pool;
    [SerializeField] List<Ability> _abilitySlot = new List<Ability>();
    private List<Ability> Abilities = new List<Ability>();
    public AnimatorController animatorController;

    public Pool Pool { get { return _pool; } set { _pool = value; } }

    public List<Ability> AbilitySlot { get { return Abilities; } set { _abilitySlot = value; } }

    public void Initialize(GameObject owner)
    {
        Debug.Log("weapon init");
        for (int i = 0; i < _abilitySlot.Count; i++)
        {
            Ability ab = Instantiate(_abilitySlot[i]);
            ab.Initialize(owner);
            Abilities.Add(ab);
        }
    }

    public void Uninitialize()
    {
        for (int i = 0; i < _abilitySlot.Count; i++)
        {
            _abilitySlot[i].Uninitialize();
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