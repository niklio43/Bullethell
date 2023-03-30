using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.RandomUtilities;

namespace BulletHell.Enemies.Collections
{
    public class EnemyCollectionGroup
    {
        public List<EnemyCollection> Collections;
        RandomList<EnemyCollection> _randomCollection;

        public EnemyCollectionGroup(SOEnemyCollectionGroup data)
        {
            Collections = new List<EnemyCollection>();
            _randomCollection = new RandomList<EnemyCollection>();

            foreach (var entry in data.Collections) {
                EnemyCollection collection = new EnemyCollection(entry.Collection);

                Collections.Add(collection);
                _randomCollection.AddEntry(collection, entry.Weight);
            }
        }

        public EnemyCollection RandomCollection()
        {
            return _randomCollection.GetRandom();
        }
    }
}
