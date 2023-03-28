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
        #region Private Fields
        [SerializeField] int _amountOfWaves;

        [Space(10)]
        [SerializeField] List<SpawnPoint> _spawnPoints;
        List<EnemySpawner> _spawners = new List<EnemySpawner>();

        List<EnemySpawner> _activeSpawners = new List<EnemySpawner>();
        int _currentWave = 0;
        #endregion

        #region Public Methods
        public RoomEnemySpawnEvent(RoomEventQueue queue, string name) : base(queue, name) { }
        #endregion

        #region Private Methods
        protected override void StartEvent()
        {
            _spawners = new List<EnemySpawner>();
            foreach (SpawnPoint point in _spawnPoints) {
                EnemyCollectionGroup group = _room.Manager.EnemyCollectionGroup;
                Vector2 position = (Vector2)_room.transform.position + point.Position;

                EnemySpawner newSpawner = new EnemySpawner(group, position, point.Radius, point.MinAmount, point.MaxAmount);
                _spawners.Add(newSpawner);
            }

            SpawnWave();
        }

        private void SpawnWave()
        {
            _currentWave++;

            if(_currentWave > _amountOfWaves) {
                Done();
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
        public override void DrawGizmosSelected(Transform transform)
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
