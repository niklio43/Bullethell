using BulletHell.Abilities;
using BulletHell.Emitters;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.InventorySystem
{
    public abstract class Weapon : InventoryItemData
    {
        #region Private Fields
        Pool _pool;
        [SerializeField] Ability _baseAbility;
        [SerializeField] List<Ability> _abilities = new List<Ability>();
        #endregion

        #region Public Fields
        public AnimatorOverrideController AnimatorController;
        public Pool Pool { get { return _pool; } set { _pool = value; } }

        public List<Ability> Abilities { get { return _abilities; } set { _abilities = value; } }
        public Ability BaseAbility { get { return _baseAbility; } }
        #endregion

        #region Public Methods
        public void Initialize(GameObject owner, GameObject host)
        {
            _baseAbility = Instantiate(_baseAbility);
            _baseAbility.Initialize(owner, host);
            for (int i = 0; i < _abilities.Count; i++)
            {
                AddAbility(_abilities[i], owner, host);
            }
        }

        public void Uninitialize()
        {
            for (int i = 0; i < _abilities.Count; i++)
            {
                _abilities[i].Uninitialize();
            }
        }

        public void AddAbility(Ability ability, GameObject owner, GameObject host)
        {
            var ab = Instantiate(ability);

            ab.Initialize(owner, host);

            for (int i = 0; i < _abilities.Count; i++)
            {
                if(_abilities[i].Id == ab.Id)
                {
                    _abilities.RemoveAt(i);
                    _abilities.Insert(i, ab);
                    return;
                }
            }
            _abilities.Add(ab);
        }
        #endregion
    }
}