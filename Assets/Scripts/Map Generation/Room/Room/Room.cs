using BulletHell.Map.Generation;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.GameEventSystem;
using BulletHell.Map.RoomEvents;

namespace BulletHell.Map
{
    [RequireComponent(typeof(RoomEventQueue))]
    public class Room : MonoBehaviour
    {
        #region Public Fields
        [Header("Events")]
        public SOGameEvent OnRoomEnter;
        public SOGameEvent OnRoomCleared;

        [Header("Config")]
        public Color ColorCoding;
        public Sprite Icon;
        public RoomCell[] Cells;

        [HideInInspector] public Vector2Int GridPosition;
        [HideInInspector] public LevelManager Manager;
        [HideInInspector] public List<Door> Doors;
        #endregion

        #region Private Fields
        RoomState _roomState = RoomState.InActive;  
        RoomEventQueue _eventQueue;
        #endregion

        #region Public Methods
        public void Initialize(LevelManager manager)
        {
            Manager = manager;

            foreach (RoomCell cell in Cells) {
                cell.Initialize(this);
            }
        }

        public void OnCreation(Vector2Int gridPosition)
        {
            GridPosition = gridPosition;
        }

        public Vector2Int GetCenterPosition()
        {
            Vector2Int avg = GridPosition;

            for (int i = 0; i < Cells.Length; i++) {
                avg += Cells[i].Pos;
            }

            return avg / Cells.Length;
        }

        public void RoomCleared()
        {
            _roomState = RoomState.Cleared;
            OnRoomCleared.Raise(this, this);
        }
        #endregion

        #region Private Methods
        private void Awake()
        {
            _eventQueue = GetComponent<RoomEventQueue>();

            for (int i = 0; i < Cells.Length; i++) {
                Doors.AddRange(Cells[i].Doors);
            }
        }

        private void PlayerEnter()
        {
            if (_roomState != RoomState.InActive) { return; }
            _roomState = RoomState.Active;
            _eventQueue.StartQueue();
            OnRoomEnter.Raise(this, this);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player")) {
                PlayerEnter();
            }
        }
        #endregion

        #region Gizmos
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            if(Cells == null) { return; }

            for (int i = 0; i < Cells.Length; i++) {
                Vector2 pos = (Vector2)transform.position + (Cells[i].Pos * GenerationUtilities.CellSize);

                Gizmos.DrawWireCube(pos, Vector2.one * GenerationUtilities.CellSize);
            }
        }
        #endregion
    }
    public enum RoomState
    {
        InActive,
        Active,
        Cleared
    }
}
