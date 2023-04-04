using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.Map.RoomEvents
{
    public class RoomCloseEvent : RoomEvent
    {
        public override void StartEvent(Room room)
        {
            foreach (Door door in room.Doors) {
                door.BlockDoor();
            }

            _completed = true;
        }
    }
}
