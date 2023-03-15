using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BulletHell.Enemies
{
    [System.Serializable]
    public class EnemyCollectionMember
    {
        [SerializeField] Enemy _object;
        [SerializeField] int _weight;
        public Enemy Object => _object;
        public int Weight => _weight;
    }
}