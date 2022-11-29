using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell;

namespace BulletHell.Emitters
{
    public abstract class BaseEmitter : MonoBehaviour
    {
        protected float interval = 0;
        protected Pool<Projectile> pool;

        [Header("Projectile")]
        public Projectile projectilePrefab;

        [Header("General")]
        public float timeToLive = 5;
        [Range(0.01f, 5f)] public float coolOffTime = .1f;
        public bool autoFire = true;
        public Vector2 direction = Vector2.up;
        [Range(0.01f, 10f)] public float speed = 1;
        [Range(1f, 100f)] public float maxSpeed = 100;
        public float rotationSpeed = 0;
        public int maxProjectiles = 10;

        [Header("Modifiers")]
        public Vector2 gravity = Vector2.zero;
        public float acceleration = 0;

        public void Initialize()
        {
            pool = new Pool<Projectile>(CreateProjectile, maxProjectiles);
        }

        public void UpdateEmitter(float dt)
        {
            if(interval > 0) {
                interval -= dt;
            }

            UpdateProjectiles(dt);
            if (autoFire) {
                interval += coolOffTime;
                FireProjectile(direction);
            }
        }

        Projectile CreateProjectile()
        {
            Projectile projectile = Instantiate(projectilePrefab);
            projectile.name = $"{projectilePrefab.name} (Pooled)";
            projectile.pool = pool;

            return projectile;
        }
        protected abstract ProjectileData FireProjectile(Vector2 direction);
        protected abstract void UpdateProjectile(ProjectileData projectile);
        protected virtual void UpdateProjectiles(float dt)
        {
            for (int i = 0; i < pool.active.Count; i++) {
                UpdateProjectile(pool.active[i].data);
            }
        }

        protected void ReturnProjectile(Projectile projectile) => projectile.ResetObject();
        public void ClearAllProjectiles() => pool.Dispose();
        private void OnDisable() => ClearAllProjectiles();
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
    }
}
