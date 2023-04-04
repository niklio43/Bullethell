using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.Map.RoomEvents
{
    public class RoomOpenEvent : RoomEvent
    {
        public override void StartEvent(Room room)
        {
            foreach (Door door in room.Doors) {
                door.UnBlockDoor();
            }

            _completed = true;
        }
    }
}
