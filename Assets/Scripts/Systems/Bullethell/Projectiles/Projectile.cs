using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell;

namespace BulletHell
{
    public class Projectile : MonoBehaviour, IPoolable
    {
        public Pool<Projectile> pool;
        public ProjectileData data = new ProjectileData();

        public void Dispose() => Destroy(gameObject);
        public void ResetObject()
        {
            data = new ProjectileData();
            transform.position = Vector3.zero;
            gameObject.SetActive(false);
            pool.Release(this);
        }
        private void FixedUpdate()
        {
            transform.position = data.position;

            if(data.timeToLive <= 0) {
                ResetObject();
            }
        }
    }
}
