using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.Enemies
{
    public class EnemyCollectionSpawner : MonoBehaviour
    {
        [SerializeField] EnemyCollectionGroup group;

        private void Awake()
        {
            group.Initialize();
        }


        float _radius = 1;

        [ContextMenu("Test")]
        public void Spawn()
        {
            EnemyWave wave = new EnemyWave(group, 3);
            wave.OnWaveCleared += Test;
            wave.StartWave();
        }

        public void Test()
        {
            Debug.Log("Wave Cleared");
        }




    }

    public class EnemyWave
    {
        public delegate void WaveClearedDelegate();
        public event WaveClearedDelegate OnWaveCleared;

        Queue<EnemyCollection> _queue;

        public EnemyWave(EnemyCollectionGroup group, int size)
        {
            Dictionary<EnemyCollection, int> collectionAmountPairs = new Dictionary<EnemyCollection, int>();

            for (int i = 0; i < size; i++) {
                while (true) {
                    EnemyCollection r = group.GetRandomCollection();

                    if (!collectionAmountPairs.ContainsKey(r)) {
                        collectionAmountPairs.Add(r, 1);
                        break;
                    }
                    else if (collectionAmountPairs[r] < r.MaxAmount || r.MaxAmount == -1) {
                        collectionAmountPairs[r]++;
                        break;
                    }
                }
            }

            _queue = new Queue<EnemyCollection>();

            foreach (var pair in collectionAmountPairs) {
                for (int i = 0; i < pair.Value; i++) {
                    _queue.Enqueue(pair.Key);
                }
            }
        }

        public void StartWave()
        {
            NextWave();
        }
        
        public void NextWave()
        {
            Debug.Log(_queue.Count);
            if(_queue.Count <= 0) {
                OnWaveCleared?.Invoke();
                return;
            }

            EnemyCollection collection = _queue.Dequeue();
            collection.Spawn(Vector2.zero, 3);
            collection.OnCleared += ClearedWave;
        }

        public void ClearedWave(EnemyCollection collection)
        {
            collection.OnCleared -= ClearedWave;
            NextWave();
        }

    }
}
