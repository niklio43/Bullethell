using System.Collections.Generic;
using UnityEngine;
using BulletHell.Emitters.Projectiles;

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

        public virtual Projectile[] FireProjectile(GameObject owner = null, Transform target = null)
        {
            Projectile[] projectiles = new Projectile[_data.EmitterPoints];

            for (int i = 0; i < _data.EmitterPoints; i++) {
                Projectile projectile = ProjectileManager.Instance.Get();
                projectile.gameObject.SetActive(true);
                _activeProjectiles.Add(projectile);


                //VEL
                projectile.transform.position = _owner.transform.position;
                projectile.Target = target;
                projectile.Velocity = _emitterGroups[i].Direction;
                projectile.LifeTime = _data.LifeTime;

                projectile.Initialize(_data.ProjectileData, owner);
                projectiles[i] = projectile;
            }

            return projectiles;
        }

        protected void ReturnProjectile(Projectile projectile) => projectile.ResetObject();
    }
}
