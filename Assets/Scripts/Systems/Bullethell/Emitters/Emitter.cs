using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.Emitters
{
    public class Emitter : MonoBehaviour
    {
        public bool AutoFire = true;
        public EmitterData Data;

        EmitterGroupsManager _emitterGroups;
        public ObjectPool<Projectile> _pool { get; private set; }
        float _interval = 0;

        private void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            Data = Instantiate(Data);
            _pool = new ObjectPool<Projectile>(CreateProjectile, Data.MaxProjectiles, name);
            _emitterGroups = new EmitterGroupsManager(transform);
        }

        private void FixedUpdate()
        {
            Data.CenterRotation += Time.fixedDeltaTime * Data.RotationSpeed * 10;
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
            for (int i = 0; i < Mathf.Clamp(Data.EmitterPoints, 0, Data.MaxProjectiles); i++) {
                Projectile projectile = _pool.Get();

                EmitterModifier modifier = _emitterGroups[i].Modifier;

                ProjectileData projectileData = Data.ProjectileData;
                float speed = Data.Speed;
                float timeToLive = Data.TimeToLive;
                float gravity = Data.Gravity;
                float acceleration = Data.Acceleration;
                Vector2 gravityPoint = Data.GravityPoint;

                if (modifier != null) {
                    if(modifier.ProjectileData != null)
                        projectileData = modifier.ProjectileData;

                    gravity = modifier.Gravity;
                    gravityPoint = modifier.GravityPoint;
                    speed = modifier.Speed;
                    acceleration = modifier.Acceleration;
                }

                projectile.Initialize(projectileData);

                projectile.transform.position = _emitterGroups[i].Position;

                projectile.Position = _emitterGroups[i].Position;
                projectile.TimeToLive = timeToLive;
                projectile.Speed = speed;
                projectile.Acceleration = acceleration;
                projectile.Gravity = gravity;
                projectile.GravityPoint = gravityPoint;
                projectile.Direction = _emitterGroups[i].Direction;
                projectile.Velocity = _emitterGroups[i].Direction * speed;
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
            Vector2 gravity = (((Vector2)transform.position + projectile.GravityPoint) - projectile.Position).normalized * projectile.Gravity;
            projectile.Velocity += (projectile.Direction * projectile.Acceleration * dt) + gravity;
            projectile.Position += projectile.Velocity * dt;
            projectile.TimeToLive -= dt;
        }

        protected void ReturnProjectile(Projectile projectile) => projectile.ResetObject();
        public void ClearAllProjectiles() => _pool.Dispose();
        private void OnDisable() => ClearAllProjectiles();
    }
}
