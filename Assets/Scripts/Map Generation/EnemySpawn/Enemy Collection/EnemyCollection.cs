using BulletHell.RandomUtilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.Enemies.Collections
{
    public class EnemyCollection
    {
        #region Public Fields
        public List<Enemy> Enemies;
        #endregion

        #region Private Fields
        RandomList<Enemy> _randomEnemy;
        #endregion

        #region Public Methods
        public EnemyCollection(SOEnemyCollection data)
        {
            Enemies = new List<Enemy>();
            _randomEnemy = new RandomList<Enemy>();

            foreach (var entry in data.Members) {
                Enemies.Add(entry.Object);
                _randomEnemy.AddEntry(entry.Object, entry.Weight);
            }
        }

        public Enemy RandomEnemy()
        {
            return _randomEnemy.GetRandom();
        }
        #endregion
    }
}
