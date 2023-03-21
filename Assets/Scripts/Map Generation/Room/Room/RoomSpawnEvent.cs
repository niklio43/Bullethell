using BulletHell.Enemies;
using BulletHell.Enemies.Spawning;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BulletHell.Map
{
    public class RoomSpawnEvent : MonoBehaviour, IRoomEvent
    {
        #region Private Fields
        [SerializeField] UnityEvent OnCompleted;
        [SerializeField] List<SpawnPoint> _spawnPoints;
        List<SpawnPoint> _activeSpawnPoints;
        Room _owner;
        #endregion

        #region Public Methods
        public void OnRoomInitialize(Room room)
        {
            _owner = room;
            _activeSpawnPoints = new List<SpawnPoint>();
            _owner.OnPlayerEnter += StartWave;

            foreach (SpawnPoint spawnPoint in _spawnPoints) {
                spawnPoint.Initialize(gameObject, room.Manager.Enemies);
                spawnPoint.OnWaveCompleted += OnSpawnPointCompleted;
            }
        }
        #endregion

        #region Private Methods

        [ContextMenu("TEST")]
        void StartWave()
        {
            _owner.CloseRoom();
            foreach (SpawnPoint spawnPoint in _spawnPoints) {
                _activeSpawnPoints.Add(spawnPoint);
                spawnPoint.SpawnWave();
            }
        }

        void OnSpawnPointCompleted(SpawnPoint spawnPoint)
        {
            _activeSpawnPoints.Remove(spawnPoint);

            if (_activeSpawnPoints.Count <= 0) {
                if (Completed()) {
                    OnCompleted?.Invoke();
                    return;
                }
                StartWave();
            }
        }

        bool Completed()
        {
            bool check = true;
            foreach (SpawnPoint spawnPoint in _spawnPoints) {
                if (!spawnPoint.Completed) check = false;
            }

            return check;
        }

        #endregion

        #region Gizmos
        private void OnDrawGizmosSelected()
        {
            if (_spawnPoints == null) { return; }
            Gizmos.color = Color.red;
            foreach (SpawnPoint spawnPoint in _spawnPoints) {
                Gizmos.DrawWireSphere((Vector2)transform.position + spawnPoint.RawPosition, spawnPoint.Radius);
            }
        }
        #endregion
    }

    [System.Serializable]
    public class SpawnPoint
    {
        #region Public Fields
        public Vector2 RawPosition => _position;
        public Vector2 Position => (Vector2)_owner.transform.position + _position;
        public float Radius => _radius;

        public Action<SpawnPoint> OnWaveCompleted;
        public bool Completed { get; private set; } = false;
        #endregion

        #region Private Fields
        [SerializeField] Vector2 _position;
        [SerializeField] float _radius;
        [SerializeField] int minAmountPerWave;
        [SerializeField] int maxAmountPerWave;
        [SerializeField] int amountOfWaves;
        EnemySpawner _spawner;
        GameObject _owner;
        #endregion

        #region Public Methods
        public void Initialize(GameObject owner, EnemyCollectionGroup group)
        {
            _owner = owner;
            _spawner = new EnemySpawner(group, Position, _radius, amountOfWaves, minAmountPerWave, maxAmountPerWave);
            _spawner.OnWaveFinished(() => { OnWaveCompleted?.Invoke(this); });
            _spawner.OnCompleted(() => { Completed = true;});
        }

        public void SpawnWave()
        {
            _spawner.NextWave();
        }
        #endregion
    }
}
