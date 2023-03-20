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

        [Header("Generation Order:")]
        [Space(-10)]
        [Header("Special -> Big -> Small -> Default.")]

        [Header("Rooms Tileset")]
        public SpecialRoomConfig[] SpecialRooms;
        public RoomConfig[] BigRooms;
        public RoomConfig[] SmallRooms;
        public RoomConfig[] DefaultRooms;

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

    [System.Serializable]
    public class RoomConfig
    {
        #region Public Fields
        public int MaxAmount => _maxAmount;
        public Room RoomOriginal => _roomOriginal;
        #endregion

        #region Private Fields
        [Tooltip("-1 = No limit")]
        [SerializeField] int _maxAmount = -1;
        [SerializeField] Room _roomOriginal;
        #endregion
    }

    [System.Serializable]
    public class SpecialRoomConfig
    {
        #region Public Fields
        public int OccupyingSquare => _occupyingSquare;
        public Room RoomOriginal => _roomOriginal;
        #endregion

        #region Private Fields
        [SerializeField] Room _roomOriginal;
        [Tooltip("-1 = No limit")]
        [SerializeField] int _occupyingSquare = 1;
        #endregion
    }
}
