using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<GameObject> _enemies = new List<GameObject>();
    [SerializeField, Range(0f, 100f)] float _spawnRadius = 5f;
    [SerializeField] int _enemiesPerWave = 5;
    Transform player;

    bool spawned = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if(Vector2.Distance(player.position, transform.position) <= _spawnRadius && !spawned)
        {
            StartWave();
            spawned = true;
        }
    }

    public void StartWave()
    {
        for (int i = 0; i < _enemiesPerWave; i++)
        {
            GetPosition();
        }
    }

    bool GetPosition()
    {
        Vector2 point = (Vector2)transform.position + (Random.insideUnitCircle * _spawnRadius);

        if (!CalculateOverlap(point)) { GetPosition(); return false; }

        SpawnEnemy(point);

        return true;
    }

    bool CalculateOverlap(Vector2 point)
    {
        RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, point, Vector2.Distance(transform.position, point));

        Debug.DrawLine(transform.position, point, Color.cyan, 5f);

        for (int i = 0; i < hit.Length; i++)
        {
            if (hit[i].collider == null) { return true; }

            if (hit[i].collider.isTrigger) { return true; }

            Debug.Log(hit[i].collider.name);
        }

        return true;
    }

    void SpawnEnemy(Vector2 point)
    {
        Instantiate(_enemies[Random.Range(0, _enemies.Count)], point, Quaternion.identity);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _spawnRadius);
    }
}
