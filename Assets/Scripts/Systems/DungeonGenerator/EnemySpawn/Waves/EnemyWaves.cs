using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.Enemies.Spawning
{
    using Random = UnityEngine.Random;
    public class EnemyWaves
    {
        #region Private Fields
        Queue<EnemyCollection> _queue;
        List<Enemy> _activeEnemies;

        Vector2 _position;
        float _radius;

        Action _onWaveStarted;
        Action _onWaveFinished;
        Action _onCompleted;
        #endregion

        #region Public Methods
        public EnemyWaves(Queue<EnemyCollection> Queue, Vector2 pos, float radius)
        {
            _queue = Queue;
            _position = pos;
            _radius = radius;
        }

        public void OnWaveStarted(Action evt) => _onWaveStarted += evt;
        public void OnWaveFinished(Action evt) => _onWaveFinished += evt;
        public void OnCompleted(Action evt) => _onCompleted += evt;

        public void NextWave()
        {
            if(_queue.Count <= 0) { 
                _onCompleted?.Invoke(); 
                _onWaveFinished?.Invoke();
                return; 
            }

            _activeEnemies = Spawn();
            foreach (Enemy enemy in _activeEnemies) {
                enemy.OnDeath += EnemyKilled;
            }
        }
        #endregion

        #region Private Methods
        List<Enemy> Spawn()
        {
            List<Enemy> activeEnemies = new List<Enemy>();
            List<Enemy> enemiesToSpawn = _queue.Dequeue().GetEnemies();
            Vector2 center = _position + (Random.insideUnitCircle * _radius);

            for (int i = 0; i < enemiesToSpawn.Count; i++) {
                while (true) {
                    Vector2 position = center + Random.insideUnitCircle * Random.Range(0, 3);

                    var collider = Physics2D.OverlapCircle(position, 1f);

                    if (collider == null || collider.isTrigger) {
                        activeEnemies.Add(EntitySpawner.SpawnEnemy(enemiesToSpawn[i], position));
                        break;
                    }
                }
            }

            return activeEnemies;
        }

        void EnemyKilled(Enemy enemy)
        {
            _activeEnemies.Remove(enemy);
            if (_activeEnemies.Count <= 0) {
                _onWaveFinished?.Invoke();
            }
        }
        #endregion
    }
}
