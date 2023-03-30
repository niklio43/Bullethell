using UnityEngine;
using BulletHell.RandomUtilities;

namespace BulletHell.Enemies.Collections
{
    [CreateAssetMenu(fileName = "Enemy Collection Group", menuName = "Enemies/Enemy Collection/New Enemy Collection Group", order = 1)]
    public class SOEnemyCollectionGroup : ScriptableObject
    {
        #region Public Fields
        public Entry[] Collections;

        [System.Serializable]
        public struct Entry
        {
            public SOEnemyCollection Collection;
            public int Weight;
        }
        #endregion

        #region Public Methods
        public RandomList<Entry> GetRandomList()
        {
            RandomList<Entry> list = new RandomList<Entry>();
            foreach (var item in Collections) {
                list.AddEntry(item, item.Weight);
            }

            return list;
        }
        #endregion
    }
}
