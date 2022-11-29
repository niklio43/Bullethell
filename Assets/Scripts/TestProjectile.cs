using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using BulletHell.ObjectPool;

public class TestProjectile : MonoBehaviour, IPoolable
{
    public Pool<TestProjectile> pool;
    public float timeToLive = 0;

    public void ResetObject()
    {
        transform.localPosition = Vector3.zero;
        gameObject.SetActive(false);
        pool.Release(this);
    }

    void Update()
    {
        timeToLive -= Time.deltaTime;
        transform.position += Vector3.one * Time.deltaTime;

        if (timeToLive <= 0) {
            ResetObject();
        }
    }
}
