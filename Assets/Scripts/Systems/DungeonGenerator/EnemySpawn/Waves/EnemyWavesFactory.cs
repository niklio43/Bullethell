using System;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.Enemies.Spawning
{
    using Random = UnityEngine.Random;
    public static class EnemyWavesFactory
    {
        #region Public Methods

        public static EnemyWaves CreateWave(EnemyCollectionGroup group, Vector2 position, float radius, int size)
        {
            return new EnemyWaves(CreateQueue(group, size), position, radius);
        }
        #endregion

        #region Private Methods
        static Queue<EnemyCollection> CreateQueue(EnemyCollectionGroup group, int size)
        {
            Dictionary<EnemyCollection, int> collectionAmountPairs = new Dictionary<EnemyCollection, int>();
            Queue<EnemyCollection> queue = new Queue<EnemyCollection>();

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

            foreach (var pair in collectionAmountPairs) {
                for (int i = 0; i < pair.Value; i++) {
                    queue.Enqueue(pair.Key);
                }
            }

            return queue;
        }
        #endregion
    }
}
