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
        public EnemyCollectionGroup Enemies => _config.EnemyCollectionGroup;
        public delegate void OnInitializeDelegate();
        public static event OnInitializeDelegate OnInitialize;
        #endregion

        #region Private Fields
        [SerializeField] GenerationConfig _Config;
        static GenerationConfig _config;
        static List<Room> _rooms;
        Room _activeRoom;
        #endregion

        protected override void OnAwake()
        {
            _config = _Config;
            Enemies.Initialize();
            BeginGeneration();
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
                room.Initialize(Instance);
                room.transform.parent = Instance.transform;
            }
            OnInitialize?.Invoke();
        }
    }
}
