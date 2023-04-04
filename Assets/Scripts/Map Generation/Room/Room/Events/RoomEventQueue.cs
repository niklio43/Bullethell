using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.Map.RoomEvents
{
    public class RoomEventQueue : MonoBehaviour
    {
        #region Public Fields
        [SerializeField] private List<OnIntializeRoomEvent> RoomEventsOnInitialize;
        [SerializeField] private List<QueuedRoomEvent> RoomEvents;
        #endregion

        #region Private Fields
        private Room _room;
        #endregion

        #region Public Methods

        public void OnInitialize(Room room)
        {
            foreach (var roomEvent in RoomEventsOnInitialize) {
                roomEvent.Event.StartEvent(room);
            }
        }


        public void StartQueue(Room room)
        {
            _room = room;
            Queue<QueuedRoomEvent> queue = new Queue<QueuedRoomEvent>(RoomEvents);
            StartCoroutine(QueueRoutine(queue));
        }


        //_room.RoomCleared();
        #endregion

        #region Private Methods
        IEnumerator QueueRoutine(Queue<QueuedRoomEvent> queue)
        {
            while (queue.Count > 0) {
                yield return IterateQueue(queue);
            }

            _room.RoomCleared();
        }

        IEnumerator IterateQueue(Queue<QueuedRoomEvent> queue)
        {
            QueuedRoomEvent top = queue.Dequeue();
            yield return new WaitForSeconds(top.Delay);
            top.Event.StartEvent(_room);

            while (queue.Peek().StartPolicy == StartPolicy.With) {
                top = queue.Dequeue();
                yield return new WaitForSeconds(top.Delay);
                top.Event.StartEvent(_room);
            }

            yield return new WaitUntil(() => top.Event.Completed);
        }
        #endregion

        [System.Serializable]
        private struct OnIntializeRoomEvent
        {
            public RoomEvent Event;
            [TextArea]
            public string Note;
        }

        [System.Serializable]
        private struct QueuedRoomEvent
        {
            public RoomEvent Event;
            [Header("Details")]
            public StartPolicy StartPolicy;
            public float Delay;
            [TextArea]
            public string Note;
        }

        private enum StartPolicy
        {
            With,
            After
        }
    }
}
