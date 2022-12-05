using System.Collections.Generic;
using UnityEngine;
using static BulletHell.Utilities;

namespace BulletHell.Emitters
{
    public class Emitter : MonoBehaviour
    {
        protected float interval = 0;
        protected Pool<Projectile> pool;
        List<EmitterGroup> emitterGroups = new List<EmitterGroup>();

        [SerializeField] bool initializeOnAwake = false;
        public EmitterData data;

        Transform bulletHolder;

        #region Data Getters
        public Vector2 direction => data.direction;
        public bool autoFire => data.autoFire;
        public int maxProjectiles => data.maxProjectiles;
        public int delay => data.delay;
        public Projectile projectilePrefab => data.projectilePrefab;
        public float timeToLive => data.timeToLive;
        public float speed => data.speed;
        public int emitterPoints => data.emitterPoints;
        public float spread => data.spread;
        public float pitch => data.pitch;
        public float radius => data.radius;
        public float centerRotation => data.centerRotation;
        #endregion

        #region LastPoll
        int _lastEmitterPoints = 0;
        float _lastRadius = 0;
        float _lastSpread = 0;
        float _lastPitch = 0;
        float _lastCenterRotation = 0;
        float _lastParentRotation = 0;
        Vector3 _lastPosition = Vector3.zero;
        #endregion

        private void Awake()
        {
            if (initializeOnAwake)
                Initialize();
        }

        public void Initialize()
        {
            GameObject go = new GameObject($"Bullet holder ({name})");
            bulletHolder = go.transform;

            pool = new Pool<Projectile>(CreateProjectile, maxProjectiles);
        }

        private void Update()
        {
            RefreshGroups();
            UpdateEmitter(Time.deltaTime);
        }

        public void UpdateEmitter(float dt)
        {
            if (interval > 0) {
                interval -= dt;
            }

            UpdateProjectiles(dt);
            if (autoFire && interval <= 0) {
                interval += delay / 1000f;
                FireProjectile();
            }
        }

        void RefreshGroups()
        {
            if (_lastParentRotation != transform.rotation.eulerAngles.z || _lastPosition != transform.position ||_lastEmitterPoints != emitterPoints || _lastRadius != radius || _lastSpread != spread || _lastPitch != pitch || _lastCenterRotation != centerRotation) {
                CreateGroups();

                for (int i = 0; i < emitterPoints; i++) {
                    float rotation = CalculateGroupRotation(i, spread) + centerRotation + transform.rotation.eulerAngles.z - (spread * ((emitterPoints - 1) / 2f));
                    Vector2 positon = (Rotate(this.direction, rotation).normalized * radius) + (Vector2)transform.position;
                    Vector2 direction = Rotate(this.direction, rotation + pitch).normalized;
                    emitterGroups[i].Set(positon, direction);
                }
            }

            _lastEmitterPoints = emitterPoints;
            _lastRadius = radius;
            _lastSpread = spread;
            _lastPitch = pitch;
            _lastCenterRotation = centerRotation;
            _lastParentRotation = transform.rotation.eulerAngles.z;
            _lastPosition = transform.position;
        }

        void CreateGroups()
        {
            if (_lastEmitterPoints == emitterPoints) { return; }

            if (emitterPoints > emitterGroups.Count) {
                int amountToCreate = emitterPoints - emitterGroups.Count;
                for (int i = 0; i < amountToCreate; i++) {
                    emitterGroups.Add(new EmitterGroup());
                }
            }
            else if (emitterPoints < emitterGroups.Count) {
                emitterGroups.RemoveRange(emitterPoints, emitterGroups.Count - emitterPoints);
            }
        }

        Projectile CreateProjectile()
        {
            Projectile projectile = Instantiate(projectilePrefab);
            projectile.name = $"{projectilePrefab.name} (Pooled)";
            projectile.pool = pool;

            return projectile;
        }
        public virtual void FireProjectile()
        {
            for (int i = 0; i < emitterPoints; i++) {
                Projectile projectile = pool.Get();
                projectile.transform.parent = bulletHolder.transform;
                ProjectileData projectileData = projectile.data;

                projectile.gameObject.SetActive(true);
                projectile.transform.position = emitterGroups[i].position;
                projectileData.position = emitterGroups[i].position;
                projectileData.speed = speed;
                projectileData.velocity = emitterGroups[i].direction * speed;
                projectileData.timeToLive = timeToLive;
            }
        }

        protected virtual void UpdateProjectiles(float dt)
        {
            for (int i = 0; i < pool.active.Count; i++) {
                UpdateProjectile(pool.active[i].data, dt);
            }
        }
        protected virtual void UpdateProjectile(ProjectileData projectile, float dt)
        {
            projectile.position += projectile.velocity * dt;
            projectile.timeToLive -= dt;
        }

        float CalculateGroupRotation(int index, float spread) => index * spread;
        protected void ReturnProjectile(Projectile projectile) => projectile.ResetObject();
        public void ClearAllProjectiles() => pool.Dispose();

        private void OnDisable() => ClearAllProjectiles();
    }
}
