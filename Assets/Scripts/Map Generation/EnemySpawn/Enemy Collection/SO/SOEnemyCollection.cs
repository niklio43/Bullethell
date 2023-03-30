using UnityEngine;
using BulletHell.RandomUtilities;

namespace BulletHell.Enemies.Collections
{
    [CreateAssetMenu(fileName = "Enemy Collection", menuName = "Enemies/Enemy Collection/New Enemy Collection", order = 1)]
    public class SOEnemyCollection : ScriptableObject
    {
        public Entry[] Members;

        [System.Serializable]
        public struct Entry
        {
            public Enemy Object;
            public int Weight;
        }
    }
}
