using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell;

namespace BulletHell
{
    public class Projectile : MonoBehaviour, IPoolable
    {
        public Pool<Projectile> pool;
        public ProjectileData data;

        public void Dispose() => Destroy(gameObject);
        public void ResetObject()
        {
            transform.position = Vector3.zero;
            gameObject.SetActive(false);
            pool.Release(this);
        }
    }
}
