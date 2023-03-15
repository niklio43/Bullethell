using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.Enemies
{
    [CreateAssetMenu(fileName = "Enemy Collection", menuName = "Enemies/Enemy Collection/New Enemy Collection", order = 1)]
    public class SOEnemyCollection : ScriptableObject
    {
        #region Public Fields
        public EnemyCollectionMember[] Members => _members;
        public int Count => _members.Length;

        //Min possible amount to spawn
        public int Min => _min;
        //Max possible amount to spawn
        public int Max => _max;
        #endregion

        #region Private Fields
        [SerializeField] private EnemyCollectionMember[] _members;
        [SerializeField] private int _min, _max;
        #endregion

        public int GetTotalWeight()
        {
            int totalWeight = 0;
            foreach (EnemyCollectionMember member in _members) {
                totalWeight += member.Weight;
            }
            return totalWeight;
        }
    }
}
