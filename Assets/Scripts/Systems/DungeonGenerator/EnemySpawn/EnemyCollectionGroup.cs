using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.Enemies
{
    [System.Serializable]
    public class EnemyCollectionGroup
    {
        [SerializeField] SOEnemyCollectionGroup _data;

        private Dictionary<EnemyCollection, (float, float)> _spawnChancePairs;

        public void Initialize()
        {
            for (int i = 0; i < _data.Count; i++) {
                _data.Members[i].Initialize();
            }

            _spawnChancePairs = new Dictionary<EnemyCollection, (float, float)>();

            float total = _data.GetTotalWeight();
            float min = 0;

            for (int i = 0; i < _data.Count; i++) {
                float max = min + _data.Members[i].Weight / total;
                _spawnChancePairs.Add(_data.Members[i], (min, max));
                min = max;
            }
        }

        public EnemyCollection GetRandomCollection()
        {
            float r = Random.value;

            EnemyCollection selected = null;

            foreach (var pair in _spawnChancePairs) {
                if(r >= pair.Value.Item1 && r <= pair.Value.Item2) {
                    selected = pair.Key;
                    break;
                }
            }

            return selected;
        }


    }
}
