using BulletHell.Map.Generation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Enemies;
using BulletHell.GameEventSystem;


namespace BulletHell.Map
{
    public class LevelManager : MonoBehaviour
    {
        #region Public Fields
        public Room[] Rooms => _rooms;
        public Room ActiveRoom => _activeRoom;
        public EnemyCollectionGroup Enemies => _config.EnemyCollectionGroup;
        #endregion

        #region Private Fields
        [SerializeField] GenerationConfig _config;
        [Header("Events")]
        [SerializeField] GameEvent OnCompletedMapGeneration;
        [SerializeField] GameEvent OnPlayerMoved;

        Room[] _rooms;
        Room _activeRoom;
        #endregion

        private void Start()
        {
            BeginGeneration();
        }

        public void PlayerEnterRoom(Room room)
        {
            _activeRoom = room;
            Debug.Log(room);
            OnPlayerMoved?.Raise(this, room);
        }

        public void BeginGeneration()
        {
            Generator levelGenerator = new Generator(_config);
            levelGenerator.BeginGeneration(OnCompletedGeneration);
        }

        void OnCompletedGeneration(List<Room> rooms)
        {
            Debug.Log("Generation Completed");
            _rooms = rooms.ToArray();

            foreach (Room room in _rooms) {
                room.Initialize(this);
                room.transform.parent = transform;
                room.OnPlayerEnter += () => {
                    PlayerEnterRoom(room);
                };
            }

            OnCompletedMapGeneration.Raise(this, _rooms);
        }
    }
}
