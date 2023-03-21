using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.Abilities
{
    [System.Serializable]
    public class WeaponAbility
    {
        [SerializeField] Ability _ability;
        [SerializeField] float _cost;

        public Ability Ability => _ability;
        public float Cost => _cost;
    }
}