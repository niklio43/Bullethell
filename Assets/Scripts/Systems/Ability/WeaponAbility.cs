using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.Abilities
{
    [System.Serializable]
    public class WeaponAbility
    {
        #region Private Fields
        [SerializeField] Ability _ability;
        [SerializeField] float _cost;
        #endregion

        #region Public Fields
        public Ability Ability => _ability;
        public float Cost => _cost;
        #endregion
    }
}