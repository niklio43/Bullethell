using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell;

public class ProjectileManager : Singleton<ProjectileManager>
{
    [SerializeField] int _size = 1000;
    
    ObjectPool<Projectile> _pool;

    Projectile _projectilePrefab;

    protected override void OnAwake()
    {
        _projectilePrefab = Resources.Load<Projectile>("EmitterProjectile");
        _pool = new ObjectPool<Projectile>(CreateProjectile, _size, "Projectiles");
    }

    public Projectile Get() => _pool.Get();

    Projectile CreateProjectile()
    {
        Projectile projectile = Instantiate(_projectilePrefab);
        projectile.Pool = _pool;

        return projectile;
    }
}
