using BulletHell.Map.Generation;
using UnityEngine;


namespace BulletHell.Map
{
    public class Room : MonoBehaviour
    {
        public RoomCell[] Cells;
        public Color colorCoding;

        public delegate void OnPlayerEnterDelegate();
        public event OnPlayerEnterDelegate OnPlayerEnter;
        public delegate void OnRoomClearedDelegate();
        public event OnRoomClearedDelegate OnRoomCleared;

        RoomState roomState = RoomState.InActive;

        public enum RoomState
        {
            InActive,
            Active,
            Cleared
        }

        private void Awake()
        {
            Initialize();
        }

        void Initialize()
        {
            foreach (RoomCell cell in Cells) {
                cell.Initialize(this);
            }
        }

        public void PlayerEnter()
        {
            if(roomState != RoomState.InActive) { return; }
            roomState = RoomState.Active;
            OnPlayerEnter();
        }

        public void RoomCleared()
        {
            roomState = RoomState.Cleared;
            OnRoomCleared();
        }


        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;

            for (int i = 0; i < Cells.Length; i++) {
                Vector2 pos = (Vector2)transform.position + (Cells[i].Pos * GenerationUtilities.CellSize);

                Gizmos.DrawWireCube(pos, Vector2.one * GenerationUtilities.CellSize);
            }
        }
    }

    [System.Serializable]
    public class RoomCell
    {
        [SerializeField] Vector2Int _pos = Vector2Int.zero;
        public Door[] Doors;

        public void Initialize(Room room)
        {
            foreach(Door door in Doors) {
                door.Initialize(room);
            }
        }

        public Vector2Int Pos => _pos;
        public int x => _pos.x;
        public int y => _pos.y;
    }
}
