using UnityEngine;

namespace BulletHell.Emitters.Projectiles
{
    public class ProjectileManager : Singleton<ProjectileManager>
    {
        #region Private Fields
        [SerializeField] int _size = 1000;

        ObjectPool<Projectile> _pool;

        Projectile _projectilePrefab;
        #endregion

        #region Public Fields
        public ObjectPool<Projectile> GetPoolInstance() => _pool;
        public Projectile Get() => _pool.Get();
        #endregion

        #region Private Methods
        protected override void OnAwake()
        {
            _projectilePrefab = Resources.Load<Projectile>("EmitterProjectile");
            _pool = new ObjectPool<Projectile>(CreateProjectile, _size, "Projectiles");
        }

        Projectile CreateProjectile()
        {
            Projectile projectile = Instantiate(_projectilePrefab);
            projectile.SetPool(_pool);

            return projectile;
        }
        #endregion
    }
}
