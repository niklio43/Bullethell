using UnityEngine;

namespace BulletHell.Emitters
{
    public class Emitter : MonoBehaviour
    {
        protected float interval = 0;
        protected Pool<Projectile> pool;
        EmitterGroup[] emitterGroups;

        [SerializeField]
        EmitterData data;

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

        private void Awake()
        {
            GameObject go = new GameObject($"Bullet holder ({name})");
            bulletHolder = go.transform;
            pool = new Pool<Projectile>(CreateProjectile, maxProjectiles);
            emitterGroups = new EmitterGroup[40];
        }

        private void Update()
        {
            RefreshEmitterGroups();
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
                FireProjectile(direction);
            }
        }

        void RefreshEmitterGroups()
        {
            for (int i = 0; i < emitterGroups.Length; i++) {
                if (i > emitterPoints) { break; }
                float rotation = CalculateGroupRotation(i, spread) + centerRotation + transform.rotation.eulerAngles.z - (spread * Mathf.Floor(emitterPoints / 2f));
                Vector2 positon = Rotate(direction, rotation).normalized * radius;
                Vector2 pointDirection = Rotate(direction, rotation + pitch).normalized;

                if (emitterGroups[i] == null) {
                    emitterGroups[i] = new EmitterGroup(positon, pointDirection);
                }
                else {
                    emitterGroups[i].Set(positon, pointDirection);
                }
            }
        }

        Projectile CreateProjectile()
        {
            Projectile projectile = Instantiate(projectilePrefab);
            projectile.name = $"{projectilePrefab.name} (Pooled)";
            projectile.pool = pool;

            return projectile;
        }
        protected virtual void FireProjectile(Vector2 direction)
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

        //TODO: Move into an essential/utilities namespace
        public static Vector2 Rotate(Vector2 v, float degrees)
        {
            float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
            float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

            float tx = v.x;
            float ty = v.y;

            v.y = (cos * tx) - (sin * ty);
            v.x = (sin * tx) - (cos * ty);

            return v;
        }
        float CalculateGroupRotation(int index, float spread) => index * spread;

        protected void ReturnProjectile(Projectile projectile) => projectile.ResetObject();
        public void ClearAllProjectiles() => pool.Dispose();
        private void OnDisable() => ClearAllProjectiles();
    }
}
