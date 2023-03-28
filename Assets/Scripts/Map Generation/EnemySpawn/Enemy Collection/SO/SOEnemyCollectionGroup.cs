using UnityEngine;
using BulletHell.RandomUtilities;

namespace BulletHell.Enemies.Collections
{
    [CreateAssetMenu(fileName = "Enemy Collection Group", menuName = "Enemies/Enemy Collection/New Enemy Collection Group", order = 1)]
    public class SOEnemyCollectionGroup : ScriptableObject
    {
        public Entry[] Collections;

        [System.Serializable]
        public struct Entry
        {
            public SOEnemyCollection Collection;
            public int Weight;
        }
    }
}
