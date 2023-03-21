using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BulletHell.RandomUtilities
{
    using Random = System.Random;

    public class RandomList<T>
    {
        private struct Entry
        {
            public double AccumulatedWeight;
            public T Item;
        }

        #region Private Fields
        private List<Entry> _entries = new List<Entry>();
        private double _accumulatedWeight;
        private Random rand = new Random();
        #endregion

        #region Public Fields
        public void AddEntry(T item, double weight, int maxAmount = -1)
        {
            _accumulatedWeight += weight;
            _entries.Add(new Entry { Item = item, AccumulatedWeight = _accumulatedWeight});
        }

        public T GetRandom()
        {
            double r = rand.NextDouble() * _accumulatedWeight;

            foreach (Entry entry in _entries) {
                if(entry.AccumulatedWeight >= r) {
                    return entry.Item;
                }
            }

            return default(T);
        }
        #endregion
    }
}
