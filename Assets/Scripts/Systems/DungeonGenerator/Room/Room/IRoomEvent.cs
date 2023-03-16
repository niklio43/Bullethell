using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.Map
{
    public interface IRoomEvent
    {
        public void OnRoomInitialize(Room room);
    }
}
