using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.Enemies
{
    [CreateAssetMenu(fileName = "Enemy Collection Group", menuName = "Enemies/Enemy Collection/New Enemy Collection Group", order = 1)]
    public class SOEnemyCollectionGroup : ScriptableObject
    {
        public EnemyCollection[] Members => _collections;
        public int Count => _collections.Length;


        #region Private Fields
        [SerializeField] EnemyCollection[] _collections;
        #endregion

        public int GetTotalWeight()
        {
            int total = 0;
            foreach (EnemyCollection collection in _collections) {
                total += collection.Weight;
            }
            return total;
        }



    }
}
