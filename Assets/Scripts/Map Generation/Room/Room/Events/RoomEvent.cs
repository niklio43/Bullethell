using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.Map.RoomEvents
{
    public abstract class RoomEvent : MonoBehaviour
    {
        protected bool _completed = false;
        public bool Completed => _completed;
        public abstract void StartEvent(Room room);
    }
}
