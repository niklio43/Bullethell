using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Enemies;

namespace BulletHell.Map.Generation
{
    [CreateAssetMenu(fileName = "GenerationConfig", menuName = "Map Generation/Generation Config")]
    public class GenerationConfig : ScriptableObject
    {
        [Header("Grid")]
        public int SizeX = 10;
        public int SizeY = 10;

        [Header("Generation Settings")]
        [SerializeField] bool _randomSeed = true;
        [SerializeField] int _seed = 0;
        public int RandomWalkSteps = 4;
        public int Size = 7;

        [Header("Room Settings")]
        public int MaxBigRooms = 4;

        [Header("Rooms Tileset")]
        public Room StartRoom;
        public Room EndRoom;

        public Room[] SmallRooms;
        public Room[] BigRooms;

        [Header("Enemy Collection")]
        public EnemyCollectionGroup EnemyCollectionGroup;

        public int GetSeed()
        {
            if(_randomSeed) {
                _seed = Random.Range(0, 9999);
            }
            return _seed;
        }
    }
}
