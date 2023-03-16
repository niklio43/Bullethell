using BulletHell.Map.Generation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Enemies;


namespace BulletHell.Map
{
    public class LevelManager : MonoBehaviour
    {
        #region Public Fields
        public List<Room> Rooms => _rooms;
        public EnemyCollectionGroup Enemies => _config.EnemyCollectionGroup;
        #endregion

        #region Private Fields
        [SerializeField] GenerationConfig _config;
        List<Room> _rooms;
        Map _map;
        #endregion

        private void Awake()
        {
            Enemies.Initialize();
            BeginGeneration();
        }

        public void BeginGeneration()
        {
            Generator levelGenerator = new Generator(_config);
            levelGenerator.BeginGeneration(OnCompletedGeneration);
        }

        void OnCompletedGeneration(List<Room> rooms)
        {
            Debug.Log("Generation Completed");
            _rooms = rooms;

            foreach (Room room in _rooms) {
                room.Initialize(this);
                room.transform.parent = transform;
            }

            CreateMap();
        }

        void CreateMap()
        {
            _map = new GameObject("Map").AddComponent<Map>();
            _map.transform.parent = transform;
            _map.transform.localPosition = Vector2.right * (_config.SizeX + 10);
            _map.Initialize(this);
            _map.CreateMap();
        }
    }
}
