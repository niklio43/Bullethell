using BulletHell.Map.Generation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Enemies.Collections;
using BulletHell.GameEventSystem;


namespace BulletHell.Map
{
    public class LevelManager : MonoBehaviour
    {
        #region Public Fields
        public Room[] Rooms => _rooms;
        public Room ActiveRoom => _activeRoom;

        public EnemyCollectionGroup EnemyCollectionGroup = null;

        #endregion

        #region Private Fields
        [SerializeField] GenerationConfig _config;
        [Header("Events")]
        [SerializeField] SOGameEvent OnPlayerMoved;
        [SerializeField] SOGameEvent OnCompletedMapGeneration;

        Room[] _rooms;
        Room _activeRoom;
        #endregion

        private void Start()
        {
            GameEventManager.GetEvent("OnRoomEnter").RegisterCallBack(PlayerEnterRoom);
            EnemyCollectionGroup = new EnemyCollectionGroup(_config.EnemyCollectionGroup);
            BeginGeneration();
        }

        public void PlayerEnterRoom(Component sender, object data)
        {
            if(data is not Room) { return; }
            Room room = data as Room;

            _activeRoom = room;
            OnPlayerMoved.Raise(this, room);
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
            }

            OnCompletedMapGeneration.Raise(this, _rooms);
        }

        private void OnDestroy()
        {
            GameEventManager.GetEvent("OnRoomEnter").UnRegisterCallback(PlayerEnterRoom);
        }
    }
}
