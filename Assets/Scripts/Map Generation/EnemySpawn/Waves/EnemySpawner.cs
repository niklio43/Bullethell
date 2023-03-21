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
        Queue<Wave> _waves;
        List<Enemy> _activeEnemies;

        Vector2 _position;
        float _radius;

        Action _onWaveStarted;
        Action _onWaveFinished;
        Action _onCompleted;
        #endregion

        #region Public Methods
        public EnemySpawner(EnemyCollectionGroup group, Vector2 pos, float radius, int amountOfWaves, int min, int max)
        {
            _position = pos;
            _radius = radius;

            CreateQueue(group, amountOfWaves, min, max);
        }

        public void OnWaveStarted(Action evt) => _onWaveStarted += evt;
        public void OnWaveFinished(Action evt) => _onWaveFinished += evt;
        public void OnCompleted(Action evt) => _onCompleted += evt;

        public void NextWave()
        {
            if (_waves.Count <= 0) {
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
        private void CreateQueue(EnemyCollectionGroup group, int amountOfWaves, int min, int max)
        {
            _waves = new Queue<Wave>();
            RandomList<EnemyCollectionGroup.Entry> _a = group.GetRandomList();

            for (int i = 0; i < amountOfWaves; i++) {
                RandomList<EnemyCollection.Entry> _b = _a.GetRandom().Collection.GetRandomList();
                _waves.Enqueue(new Wave { RandomList = _b , Size = Random.Range(min, max)});
            }
        }

        private List<Enemy> Spawn()
        {
            List<Enemy> activeEnemies = new List<Enemy>();
            Wave wave = _waves.Dequeue();

            Vector2 center = _position + (Random.insideUnitCircle * _radius);

            for (int i = 0; i < wave.Size; i++) {
                while (true) {
                    Vector2 position = center + Random.insideUnitCircle * Random.Range(0, 3);

                    var collider = Physics2D.OverlapCircle(position, 1f);

                    if (collider == null || collider.isTrigger) {
                        activeEnemies.Add(EntitySpawner.SpawnEnemy(wave.RandomList.GetRandom().Object, position));
                        break;
                    }
                }
            }

            return activeEnemies;
        }

        private void EnemyKilled(Enemy enemy)
        {
            _activeEnemies.Remove(enemy);
            if (_activeEnemies.Count <= 0) {
                _onWaveFinished?.Invoke();
            }
        }


        private struct Wave
        {
            public RandomList<EnemyCollection.Entry> RandomList;
            public int Size;
        }

        #endregion

    }
}
