using UnityEngine;

namespace BulletHell.Emitters.Projectiles
{
    public class ProjectileManager : Singleton<ProjectileManager>
    {
        [SerializeField] int _size = 1000;

        ObjectPool<EmitterProjectile> _pool;

        EmitterProjectile _projectilePrefab;

        protected override void OnAwake()
        {
            _projectilePrefab = Resources.Load<EmitterProjectile>("EmitterProjectile");
            _pool = new ObjectPool<EmitterProjectile>(CreateProjectile, _size, "Projectiles");
        }

        public ObjectPool<EmitterProjectile> GetPoolInstance() => _pool;
        public EmitterProjectile Get() => _pool.Get();

        EmitterProjectile CreateProjectile()
        {
            EmitterProjectile projectile = Instantiate(_projectilePrefab);
            projectile.Pool = _pool;

            return projectile;
        }
    }
}
