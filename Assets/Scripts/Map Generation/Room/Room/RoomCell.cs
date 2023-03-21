using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BulletHell.Map
{
    [System.Serializable]
    public class RoomCell
    {
        #region Private Fields
        [SerializeField] Door[] _doors;
        [SerializeField] Vector2Int _pos = Vector2Int.zero;
        #endregion

        #region Public Methods
        public Door[] Doors => _doors;
        public void Initialize(Room room)
        {
            foreach (Door door in _doors) {
                door.Initialize(room);
            }
        }

        public Vector2Int Pos => _pos;
        public int x => _pos.x;
        public int y => _pos.y;
        #endregion
    }
}
