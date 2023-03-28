using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.Map.RoomEvents
{
    public class RoomOpenEvent : RoomEvent
    {
        #region Public Methods
        public RoomOpenEvent(RoomEventQueue queue, string name) : base(queue, name) { }
        #endregion

        #region Private Methods
        protected override void StartEvent()
        {
            foreach (Door door in _room.Doors) {
                door.UnBlockDoor();
            }

            Done();
        }
        #endregion
    }
}
