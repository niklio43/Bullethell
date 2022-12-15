using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Emitters;

namespace BulletHell.Abilities
{
    public abstract class EmitterAbility : Ability
    {
        protected Emitter _emitter;
        [SerializeField] EmitterData _emitterData;

        public override abstract void DoAbility();

        public override void Initialize(WeaponController weaponController)
        {
            _emitter = new GameObject($"{name} (Emitter)").AddComponent<Emitter>();
            _emitter.transform.SetParent(weaponController.transform);
            _emitter.transform.localPosition = Vector3.zero;
            _emitter.Data = _emitterData;
            _emitter.AutoFire = false;
        }
    }
}