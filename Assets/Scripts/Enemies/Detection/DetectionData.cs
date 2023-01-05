using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BulletHell.Enemies.Detection
{
    public class DetectionData
    {
        public EntityData[] this[string key] => Data[key];

        private Dictionary<string, EntityData[]> Data = new Dictionary<string, EntityData[]>();

        public void Clear() => Data.Clear();

        public int Count(string tag)
        {
            if(!Data.ContainsKey(tag)) { return 0; }

            return Data[tag].Length;
        }

        public void Add(string key, EntityData[] entities)
        {
            Data.Add(key, entities);
        }
    }

    public class EntityData
    {
        public Collider2D Collider;
        public Transform transform => Collider.transform;
        public GameObject gameObject => Collider.gameObject;
        public string tag => Collider.tag;

        public EntityData(Collider2D collider)
        {
            Collider = collider;
        }
    }
}
