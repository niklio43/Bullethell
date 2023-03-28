using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.Map.RoomEvents
{
    public class RoomEventQueue : MonoBehaviour
    {
        #region Public Fields
        [SerializeReference] 
        public List<RoomEvent> RoomEvents;
        #endregion

        #region Private Fields
        private Room _room;
        private int _queueIndex;
        #endregion

        #region Public Methods

        public void StartQueue()
        {
            _queueIndex = 0;
            NextInQueue();
        }

        public void NextInQueue()
        {
            if(_queueIndex >= RoomEvents.Count) {
                _room.RoomCleared();
                return;
            }

            RoomEvent lastRoomEvent = RoomEvents[_queueIndex];
            RoomEvents[_queueIndex].StartEvent(_room);
            _queueIndex++;

            while(_queueIndex < RoomEvents.Count) {
                if(RoomEvents[_queueIndex].StartPolicy == StartPolicy.After) {
                    break;
                }

                lastRoomEvent = RoomEvents[_queueIndex];
                RoomEvents[_queueIndex].StartEvent(_room);

                _queueIndex++;
            }

            lastRoomEvent.OnFinished(NextInQueue);
        }


        #region Add Events
        [ContextMenu("Add Event/Open Event")] public void AddOpenEvent() => RoomEvents.Add(new RoomOpenEvent(this, "OpenEvent"));
        [ContextMenu("Add Event/Close Event")] public void AddCloseEvent() => RoomEvents.Add(new RoomCloseEvent(this, "CloseEvent"));
        [ContextMenu("Add Event/Spawn Event")] public void AddEnemySpawnEvent() => RoomEvents.Add(new RoomEnemySpawnEvent(this, "EnemySpawnEvent"));
        #endregion

        #endregion

        #region Private Methods
        private void Awake()
        {
            _room = GetComponent<Room>();
        }
        #endregion

        #region Gizmos
        private void OnDrawGizmosSelected()
        {
            if(RoomEvents == null) { return; }
            foreach (RoomEvent roomEvent in RoomEvents) {
                if (roomEvent == null) { continue; }
                if (!roomEvent.ShowGizmos) { continue; }
                roomEvent.DrawGizmosSelected(transform);
            }
        }

        private void OnDrawGizmos()
        {
            if (RoomEvents == null) { return; }
            foreach (RoomEvent roomEvent in RoomEvents) {
                if(roomEvent == null) { continue; }
                if (!roomEvent.ShowGizmos) { continue; }
                roomEvent.DrawGizmos(transform);
            }
        }
        #endregion
    }
}
