using UnityEngine;
using BulletHell.RandomUtilities;

namespace BulletHell.Enemies
{
    [CreateAssetMenu(fileName = "Enemy Collection", menuName = "Enemies/Enemy Collection/New Enemy Collection", order = 1)]
    public class EnemyCollection : ScriptableObject
    {
        public Entry[] Members;

        [System.Serializable]
        public struct Entry
        {
            public Enemy Object;
            public int Weight;
        }

        public RandomList<Entry> GetRandomList()
        {
            RandomList<Entry> list = new RandomList<Entry>();
            foreach (var item in Members) {
                list.AddEntry(item, item.Weight);
            }

            return list;
        }
    }
}
