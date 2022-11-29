using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.ObjectPool;

public class TestEmitter : MonoBehaviour
{
    [SerializeField] TestProjectile prefab;
    Pool<TestProjectile> m_Pool;
    public Pool<TestProjectile> pool
    {
        get {
            if (m_Pool == null) {
                m_Pool = new Pool<TestProjectile>(CreateProjectile, 10);
            }
            return m_Pool;
        }
    }

    private void Start()
    {
        InvokeRepeating(nameof(Fire), 0, 4f);
    }

    void Fire()
    {
        TestProjectile projectile = pool.Get();
        projectile.timeToLive = 3;
        projectile.gameObject.SetActive(true);
    }

    TestProjectile CreateProjectile()
    {
        TestProjectile newProjectile = Instantiate(prefab, transform);
        newProjectile.name = $"{prefab.name} (Pooled))";
        newProjectile.pool = pool;
        newProjectile.gameObject.SetActive(false);

        return newProjectile;
    }
}
