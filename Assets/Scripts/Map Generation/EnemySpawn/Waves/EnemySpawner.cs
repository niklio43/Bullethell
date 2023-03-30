using BulletHell.Enemies.Collections;
using BulletHell.RandomUtilities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.Enemies.Spawning
{
    using Random = UnityEngine.Random;
    public class EnemySpawner
    {
        #region Private Fields
        private List<Enemy> _activeEnemies = new List<Enemy>();

        private EnemyCollectionGroup _group;
        private Vector2 _position;
        private float _radius;
        private int _min;
        private int _max;

        private Action<EnemySpawner> _OnCleared;
        private const int spawnAreaSize = 3;
        #endregion

        #region Public Methods
        public EnemySpawner(EnemyCollectionGroup group, Vector2 position, float radius, int min, int max)
        {
            _group = group;
            _position = position;
            _radius = radius;
            _min = min;
            _max = max;
        }

        public void Spawn()
        {
            Vector2 position = Vector2.zero;

            while (true) {
                position = _position + Random.insideUnitCircle * (_radius - spawnAreaSize);

                if(!Physics2D.OverlapCircle(position, spawnAreaSize)) {
                    break;
                }
            }

            int amount = Random.Range(_min, _max);
            List<Enemy> enemies = new List<Enemy>();

            for (int i = 0; i < amount; i++) {
                enemies.Add(_group.RandomCollection().RandomEnemy());
            }

            InternalSpawn(position, enemies);
        }
        public void OnCleared(Action<EnemySpawner> evt) => _OnCleared += evt;

        #endregion

        #region Private Methods
        private void InternalSpawn(Vector2 position, List<Enemy> enemiesToSpawn)
        {
            _activeEnemies.Clear();

            for (int i = 0; i < enemiesToSpawn.Count; i++) {
                Vector2 pos = position + Random.insideUnitCircle * Random.Range(0, spawnAreaSize);
                Enemy activeEnemy = EntitySpawner.SpawnEnemy(enemiesToSpawn[i], position);
                activeEnemy.OnDeath += EnemyKilled;

                _activeEnemies.Add(activeEnemy);
            }
        }

        private void EnemyKilled(Enemy enemy)
        {
            _activeEnemies.Remove(enemy);
            if (_activeEnemies.Count <= 0) {
                _OnCleared?.Invoke(this);
            }
        }
        #endregion
    }
}
