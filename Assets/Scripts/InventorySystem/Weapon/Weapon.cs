using BulletHell.Abilities;
using BulletHell.Emitters;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.InventorySystem
{
    public abstract class Weapon : InventoryItemData
    {
        Pool _pool;
        [SerializeField] List<Ability> _abilitySlot = new List<Ability>();
        List<Ability> _abilities = new List<Ability>();
        public AnimatorOverrideController AnimatorController;
        public Pool Pool { get { return _pool; } set { _pool = value; } }

        public List<Ability> Abilities { get { return _abilities; } set { _abilities = value; } }

        public void Initialize(GameObject owner, GameObject host)
        {
            _abilities.Clear();
            for (int i = 0; i < _abilitySlot.Count; i++)
            {
                var ab = Instantiate(_abilitySlot[i]);

                ab.Initialize(owner, host);

                _abilities.Add(ab);
            }
        }

        public void Uninitialize()
        {
            for (int i = 0; i < _abilities.Count; i++)
            {
                _abilities[i].Uninitialize();
            }
        }

        public void AddAbility(Ability ability, Weapon weapon, GameObject owner)
        {
            if (_abilities.Count >= 3) return;
            for (int i = 0; i < _abilities.Count; i++)
            {
                if (_abilities[i].Id == ability.Id) { owner.GetComponent<WeaponController>().FillAbilitySlot(weapon); return; }
            }
            _abilities.Add(ability);
            ability.Initialize(owner);
        }
    }
}