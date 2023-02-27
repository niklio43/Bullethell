using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Abilities;

namespace BulletHell.Enemies
{
    [System.Serializable]
    public class EnemyAbility
    {
        public Ability Ability;
        [SerializeField] float _minDist;
        [SerializeField] float _maxDist;

        Enemy _owner;

        public void Initialize(Enemy owner)
        {
            _owner = owner;
            Ability = GameObject.Instantiate(Ability);
            GameObject host = (_owner.Weapon != null) ? _owner.Weapon.gameObject : _owner.Aim.gameObject;
            Ability.Initialize(_owner.gameObject, host);
        }

        public void Cast(Transform target = null, System.Action castDelegate = null)
        {
            Ability.Cast(target, castDelegate);
        }

        public void UpdateAbility(float dt) => Ability.UpdateAbility(dt);

        public void Cancel() => Ability?.Cancel();

        public bool CanCast()
        {
            float dist = Vector2.Distance(_owner.transform.position, _owner.Target.position);
            return Ability.CanCast() && (dist > _minDist && dist < _maxDist);
        }
    }
}
