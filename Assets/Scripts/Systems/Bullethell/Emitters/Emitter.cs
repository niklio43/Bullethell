using System.Collections.Generic;
using UnityEngine;
using BulletHell.Stats;
using BulletHell.Emitters.Projectiles;
using BulletHell.StatusSystem;

namespace BulletHell.Emitters
{
    public class Emitter
    {
        GameObject _owner;
        Transform _transform;
        EmitterData _data;
        List<Projectile> _activeProjectiles;
        EmitterGroupsManager _emitterGroups;

        public Emitter(GameObject owner, EmitterData data)
        {
            _owner = owner;
            _data = data;

            _transform = _owner.transform;
            _activeProjectiles = new List<Projectile>();
            _emitterGroups = new EmitterGroupsManager(owner.transform, _data);
        }

        public void UpdateEmitter()
        {
            _emitterGroups.UpdateGroups();
        }

        public void SetDirection(Vector2 direction)
        {
            _data.Direction = direction;
            _emitterGroups.UpdateGroups();
        }

        public virtual void FireProjectile(Character projectileOwner = null, Transform target = null)
        {
            for (int i = 0; i < _data.EmitterPoints; i++) {
                Projectile projectile = ProjectileManager.Instance.Get();
                projectile.gameObject.SetActive(true);
                _activeProjectiles.Add(projectile);


                //VEL
                projectile.transform.position = _owner.transform.position;
                projectile.Target = target;
                projectile.Velocity = _emitterGroups[i].Direction;
                projectile.LifeTime = _data.LifeTime;

                projectile.SetOwner(projectileOwner);
                projectile.Initialize(_data.ProjectileData);
            }
        }

        protected void ReturnProjectile(Projectile projectile) => projectile.ResetObject();
    }
}
