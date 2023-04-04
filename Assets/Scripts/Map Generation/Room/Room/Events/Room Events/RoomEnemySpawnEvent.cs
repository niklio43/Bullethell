using BulletHell.Enemies;
using BulletHell.Enemies.Collections;
using BulletHell.Enemies.Spawning;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.Map.RoomEvents
{
    public class RoomEnemySpawnEvent : RoomEvent
    {
        #region Public Fields
        #endregion

        #region Private Fields
        [SerializeField] int _amountOfWaves;
        [SerializeField] List<SpawnPoint> _spawnPoints;

        List<EnemySpawner> _spawners = new List<EnemySpawner>();
        List<EnemySpawner> _activeSpawners = new List<EnemySpawner>();
        int _currentWave = 0;

        #endregion

        #region Public Methods
        public override void StartEvent(Room room)
        {
            _spawners = new List<EnemySpawner>();
            foreach (SpawnPoint point in _spawnPoints) {
                EnemyCollectionGroup group = room.Manager.EnemyCollectionGroup;
                Vector2 position = (Vector2)transform.position + point.Position;

                EnemySpawner newSpawner = new EnemySpawner(group, position, point.Radius, point.MinAmount, point.MaxAmount);
                _spawners.Add(newSpawner);
            }

            SpawnWave();
        }
        #endregion

        #region Private Methods
        private void SpawnWave()
        {
            _currentWave++;

            if(_currentWave > _amountOfWaves) {
                _completed = true;
                return;
            }

            foreach (EnemySpawner spawner in _spawners) {
                spawner.Spawn();
                _activeSpawners.Add(spawner);
                spawner.OnCleared(SpawnerCleared);
            }
        }

        private void SpawnerCleared(EnemySpawner spawner)
        {
            if (!_activeSpawners.Contains(spawner)) return;

            _activeSpawners.Remove(spawner);

            if(_activeSpawners.Count <= 0) {
                SpawnWave();
            }
        }
        #endregion

        #region Gizmos

        private void OnDrawGizmosSelected()
        {
            if (_spawnPoints == null) { return; }
            Gizmos.color = Color.red;
            foreach (SpawnPoint point in _spawnPoints) {
                Gizmos.DrawWireSphere((Vector2)transform.position + point.Position, point.Radius);

            }
        }
        #endregion

        [System.Serializable]
        private struct SpawnPoint
        {
            public Vector2 Position;
            public float Radius;
            public int MinAmount;
            public int MaxAmount;
        }
    }
}
