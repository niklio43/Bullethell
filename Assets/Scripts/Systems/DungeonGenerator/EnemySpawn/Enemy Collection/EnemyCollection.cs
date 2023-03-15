using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.Enemies
{
    [System.Serializable]
    public class EnemyCollection
    {
        #region Public Fields
        public delegate void OnClearedDelegate(EnemyCollection collection);
        public event OnClearedDelegate OnCleared;

        public int Weight => _weight;
        public int MaxAmount => _maxAmount;
        #endregion

        #region Private Fields
        [SerializeField] SOEnemyCollection _data;
        [SerializeField] int _weight;
        [SerializeField] int _maxAmount;

        private Dictionary<EnemyCollectionMember, (float, float)> _spawnChancePairs;
        private List<Enemy> _activeEnemies;
        #endregion
        public void Initialize()
        {
            _activeEnemies = new List<Enemy>();
            _spawnChancePairs = new Dictionary<EnemyCollectionMember, (float, float)>();

            float total = _data.GetTotalWeight();
            float min = 0;

            for (int i = 0; i < _data.Count; i++) {
                float max = min + _data.Members[i].Weight / total;
                _spawnChancePairs.Add(_data.Members[i], (min, max));
                min = max;
            }
        }

        public void Spawn(Vector2 position, float radius)
        {
            int amount = Random.Range(_data.Min, _data.Max);

            for (int i = 0; i < amount; i++) {
                Vector2 pos;

                while (true) {
                    pos = position + Random.insideUnitCircle * Random.Range(0, radius);

                    if (Physics2D.OverlapCircle(pos, 1f) == null) {
                        break;
                    }
                }

                EnemyCollectionMember member = GetRandomMember();
                Enemy enemy = GameObject.Instantiate(member.Object, pos, Quaternion.identity);
                _activeEnemies.Add(enemy);
                enemy.OnDeath += EnemyKilled;
            }
        }

        EnemyCollectionMember GetRandomMember()
        {
            float r = Random.value;

            EnemyCollectionMember selected = null;

            foreach (var pair in _spawnChancePairs) {
                if (r >= pair.Value.Item1 && r <= pair.Value.Item2) {
                    selected = pair.Key;
                    break;
                }
            }

            return selected;
        }

        void EnemyKilled(Enemy enemy)
        {
            _activeEnemies.Remove(enemy);

            if(_activeEnemies.Count <= 0) {
                OnCleared?.Invoke(this);
            }
        }
    }
}
