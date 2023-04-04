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

        public Vector2Int GetCenterPositionAsInt()
        {
            Vector2Int avg = Vector2Int.zero;

            for (int i = 0; i < Cells.Length; i++) {
                avg += Cells[i].Pos;
            }

            return GridPosition + avg / Cells.Length;
        }

        public Vector2 GetCenterPosition()
        {
            Vector2 avg = Vector2.zero;

            for (int i = 0; i < Cells.Length; i++) {
                avg += Cells[i].Pos;
            }

            return GridPosition + avg / Cells.Length;
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
            for (int i = 0; i < Cells.Length; i++) {
                Doors.AddRange(Cells[i].Doors);
            }

            _eventQueue = GetComponent<RoomEventQueue>();
            _eventQueue.OnInitialize(this);
        }

        public void PlayerEnter()
        {
            OnRoomEnter.Raise(this, this);
            if (_roomState != RoomState.InActive) { return; }
            _roomState = RoomState.Active;
            _eventQueue.StartQueue(this);
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
