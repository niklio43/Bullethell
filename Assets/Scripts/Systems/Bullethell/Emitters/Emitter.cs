using System.Collections.Generic;
using UnityEngine;
using static BulletHell.Utilities;

namespace BulletHell.Emitters
{
    public class Emitter : MonoBehaviour
    {
        public bool AutoFire = true;
        public EmitterData Data;


        ObjectPool<Projectile> _pool;
        List<EmitterGroup> _emitterGroups = new List<EmitterGroup>();

        float _interval = 0;

        ModifierController _modifiers => Data.Modifiers;
        float _parentRotation => transform.rotation.eulerAngles.z;
        Vector2 _direction => Vector2.up;

        private void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            _pool = new ObjectPool<Projectile>(CreateProjectile, Data.MaxProjectiles, name);
            CreateGroups();
        }

        private void Update()
        {
            RefreshGroups();
            UpdateEmitter(Time.deltaTime);
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

        void CreateGroups()
        {
            for (int i = 0; i < Data.EmitterPoints; i++) {
                _emitterGroups.Add(new EmitterGroup());
            }
        }

        void RefreshGroups()
        {
            for (int n = 0; n < Data.EmitterPoints; n++) {

                _emitterGroups[n].ClearModifier();

                float spread = n * Data.Spread;
                float pitch = Data.Pitch;
                float offset = Data.Offset;

                //float centerSpread = (spread * ((_emitterPoints - 1) / 2f));

                EmitterModifier activeModifier = null;

                for (int i = 0; i < _modifiers.Count; i++) {
                    int value = (n % _modifiers[i].Factor) - _modifiers[i].Count;
                    if (value > 0) { continue; }

                    activeModifier = _modifiers[i];
                    spread += _modifiers[i].Spread;
                    pitch += _modifiers[i].Pitch;
                    offset += _modifiers[i].Offset;
                }

                float rotation = spread + Data.CenterRotation + _parentRotation;
                Vector2 positon = (Rotate(_direction, rotation).normalized * offset) + (Vector2)transform.position;

                Vector2 direction = Rotate(_direction, rotation + pitch).normalized;

                _emitterGroups[n].Set(positon, direction);
                _emitterGroups[n].SetModifier(activeModifier);
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
                UpdateProjectile(_pool.active[i], dt);
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
