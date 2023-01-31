using System.Collections.Generic;
using UnityEngine;
using BulletHell.Stats;

namespace BulletHell.Emitters
{
    public class Emitter : MonoBehaviour
    {
        public bool AutoFire = true;
        public EmitterData Data;

        DamageInfo _damage;
        List<Projectile> _activeProjectiles;
        EmitterGroupsManager _emitterGroups;
        float _interval = 0;

        private void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            _activeProjectiles = new List<Projectile>();
            Data = Instantiate(Data);
            _emitterGroups = new EmitterGroupsManager(transform);
        }

        public void SetDamage(DamageInfo damage)
        {
            _damage = damage;
        }

        private void FixedUpdate()
        {
            Data.CenterRotation += Time.fixedDeltaTime * Data.RotationSpeed * 10;
            Data.ParentRotation = transform.rotation.eulerAngles.z;
            _emitterGroups.UpdateGroups(Data);
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


        public void SetDirection(Vector2 direction)
        {
            Data.Direction = direction;
            _emitterGroups.UpdateGroups(Data);
        }

        public virtual void FireProjectile()
        {
            for (int i = 0; i < Mathf.Clamp(Data.EmitterPoints, 0, Data.MaxProjectiles); i++) {
                Projectile projectile = ProjectileManager.Instance.Get();
                _activeProjectiles.Add(projectile);
                EmitterModifier modifier = _emitterGroups[i].Modifier;

                ProjectileData projectileData = Data.ProjectileData;
                float speed = Data.Speed;
                float timeToLive = Data.TimeToLive;
                float gravity = Data.Gravity;
                float acceleration = Data.Acceleration;
                Vector2 gravityPoint = Data.GravityPoint;

                if (modifier != null) {
                    if (modifier.ProjectileData != null) {
                        projectileData = modifier.ProjectileData;
                    }

                    gravity = modifier.Gravity;
                    gravityPoint = modifier.GravityPoint;
                    speed = modifier.Speed;
                    acceleration = modifier.Acceleration;
                }

                projectile.transform.position = _emitterGroups[i].Position;
                projectile.Position = _emitterGroups[i].Position;
                projectile.TimeToLive = timeToLive;
                projectile.Speed = speed;
                projectile.Acceleration = acceleration;
                projectile.Gravity = gravity;
                projectile.GravityPoint = gravityPoint;
                projectile.Direction = _emitterGroups[i].Direction;
                projectile.Velocity = _emitterGroups[i].Direction * speed;
                projectile.Damage = _damage;
                projectile.Initialize(projectileData);
            }
        }

        protected virtual void UpdateProjectiles(float dt)
        {
            for (int i = 0; i < _activeProjectiles.Count; i++) {
                UpdateProjectile(_activeProjectiles[i], dt);
            }
        }

        protected virtual void UpdateProjectile(Projectile projectile, float dt)
        {
            Vector2 gravity = (((Vector2)transform.position + projectile.GravityPoint) - projectile.Position).normalized * projectile.Gravity;
            projectile.Velocity += (projectile.Direction * projectile.Acceleration * dt) + gravity;
            projectile.Position += projectile.Velocity * dt;
            projectile.TimeToLive -= dt;

            if (projectile.TimeToLive <= 0) {
                projectile.ResetObject();
            }
        }

        protected void ReturnProjectile(Projectile projectile) => projectile.ResetObject();
        public void ClearAllProjectiles()
        {
            foreach (Projectile projectile in _activeProjectiles) {
                projectile.ResetObject();
            }
        }

        private void OnDisable() => ClearAllProjectiles();
    }
}
