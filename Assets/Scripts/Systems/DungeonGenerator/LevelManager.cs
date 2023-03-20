using BulletHell.Map.Generation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Enemies;


namespace BulletHell.Map
{
    public class LevelManager : Singleton<LevelManager>
    {
        #region Public Fields
        public List<Room> Rooms => _rooms;
        public Room ActiveRoom => _activeRoom;
        public static EnemyCollectionGroup Enemies => _config.EnemyCollectionGroup;
        public delegate void OnInitializeDelegate();
        public static event OnInitializeDelegate OnInitialize;
        public delegate void OnPlayerMovedDelegate(Room room);
        public static event OnPlayerMovedDelegate OnPlayerMoved;
        #endregion

        #region Private Fields
        [SerializeField] GenerationConfig _Config;
        static GenerationConfig _config;
        static List<Room> _rooms;
        static Room _activeRoom;
        #endregion

        protected override void OnAwake()
        {
            _config = _Config;
            Enemies.Initialize();
            BeginGeneration();
        }

        public static void PlayerEnterRoom(Room room)
        {
            _activeRoom = room;
            OnPlayerMoved?.Invoke(room);
        }

        public static void BeginGeneration()
        {
            Generator levelGenerator = new Generator(_config);
            levelGenerator.BeginGeneration(OnCompletedGeneration);
        }

        static void OnCompletedGeneration(List<Room> rooms)
        {
            Debug.Log("Generation Completed");
            _rooms = rooms;

            foreach (Room room in _rooms) {
                room.Initialize();
                room.transform.parent = Instance.transform;
            }
            OnInitialize?.Invoke();
        }
    }
}
