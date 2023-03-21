using UnityEngine;
using BulletHell.RandomUtilities;

namespace BulletHell.Enemies
{
    [CreateAssetMenu(fileName = "Enemy Collection Group", menuName = "Enemies/Enemy Collection/New Enemy Collection Group", order = 1)]
    public class EnemyCollectionGroup : ScriptableObject
    {
        public Entry[] Collections;

        [System.Serializable]
        public struct Entry
        {
            public EnemyCollection Collection;
            public int Weight;
        }

        public RandomList<Entry> GetRandomList()
        {
            RandomList<Entry> list = new RandomList<Entry>();
            foreach (var item in Collections) {
                list.AddEntry(item, item.Weight);
            }

            return list;
        }
    }
}
