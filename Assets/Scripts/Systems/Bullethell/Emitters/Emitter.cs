using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.Emitters
{
    public class Emitter : MonoBehaviour
    {
        public bool AutoFire = true;
        public EmitterData Data;

        EmitterGroups _emitterGroups;
        public ObjectPool<Projectile> _pool { get; private set; }
        float _interval = 0;

        private void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            _pool = new ObjectPool<Projectile>(CreateProjectile, Data.MaxProjectiles, name);
            _emitterGroups = new EmitterGroups(transform);
        }

        private void FixedUpdate()
        {
            Data.ParentRotation = transform.rotation.eulerAngles.z;
            _emitterGroups.UpdateGroups(Data, Data.Modifiers);
            UpdateEmitter(Time.fixedDeltaTime);
        }

        public void UpdateEmitter(float dt)
        {
            if (_interval > 0) {
                _interval -= dt;
            }


            UpdateProjectiles(dt);
            if (AutoFire && _interval <= 0) {
                _interval += Data.Delay / 1000f;
                FireProjectile();
            }
        }

        Projectile CreateProjectile()
        {
            Projectile projectile = Instantiate(Data.ProjectilePrefab);
            projectile.Pool = _pool;

            return projectile;
        }

        public virtual void FireProjectile()
        {
            for (int n = 0; n < Data.EmitterPoints; n++) {
                EmitterModifier modifier = _emitterGroups[n].Modifier;

                Projectile projectile = _pool.Get();

                ProjectileData projectileData = Data.ProjectileData;
                float speed = Data.BaseSpeed;
                float timeToLive = Data.TimeToLive;

                if (modifier != null) {
                    projectileData = modifier.ProjectileData;
                    speed *= modifier.SpeedMultiplier;
                }

                projectile.Initialize(projectileData);

                projectile.gameObject.SetActive(true);
                projectile.transform.position = _emitterGroups[n].Position;
                projectile.Position = _emitterGroups[n].Position;
                projectile.Speed = speed;

                projectile.Acceleration = 0;
                projectile.Velocity = _emitterGroups[n].Direction * speed;
                projectile.TimeToLive = timeToLive;
            }
        }

        protected virtual void UpdateProjectiles(float dt)
        {
            for (int i = 0; i < _pool.active.Count; i++) {
                UpdateProjectile(_pool.members[_pool.active[i]], dt);
            }
        }

        protected virtual void UpdateProjectile(Projectile projectile, float dt)
        {
            projectile.Velocity *= (1 + projectile.Acceleration * dt);
            projectile.Position += projectile.Velocity * dt;
            projectile.TimeToLive -= dt;
        }

        protected void ReturnProjectile(Projectile projectile) => projectile.ResetObject();
        public void ClearAllProjectiles() => _pool.Dispose();
        private void OnDisable() => ClearAllProjectiles();
    }
}
