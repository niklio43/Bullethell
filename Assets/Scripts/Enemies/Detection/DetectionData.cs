using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace BulletHell.Enemies.Detection
{
    public class DetectionData
    {
        #region Private Fields
        private Dictionary<string, EntityData[]> Data = new Dictionary<string, EntityData[]>();
        #endregion

        #region Public Fields
        public EntityData[] this[string key] => Data[key];
        public void Clear() => Data.Clear();
        #endregion

        #region Public Methods
        public int Count(string tag)
        {
            if(!Data.ContainsKey(tag)) { return 0; }

            return Data[tag].Length;
        }

        public void Add(string key, EntityData[] entities)
        {
            if(Data.ContainsKey(key) && Data[key] != null) {
               entities = Data[key].Concat(entities).ToArray();
               Data.Remove(key);
            }

            Data.Add(key, entities);
        }
        #endregion
    }

    public class EntityData
    {
        #region Public Fields
        public Collider2D Collider;
        public Transform transform => Collider.transform;
        public GameObject gameObject => Collider.gameObject;
        public string tag => Collider.tag;
        #endregion

        public EntityData(Collider2D collider)
        {
            Collider = collider;
        }
    }
}
