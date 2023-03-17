using BulletHell.Map.Generation;
using System.Collections.Generic;
using UnityEngine;


namespace BulletHell.Map
{
    public class Room : MonoBehaviour
    {
        #region Public Fields
        public delegate void OnPlayerEnterDelegate();
        public delegate void OnCloseRoomDelegate();
        public delegate void OnOpenRoomDelegate();
        public delegate void OnRoomClearedDelegate();

        public event OnPlayerEnterDelegate OnPlayerEnter;
        public event OnCloseRoomDelegate OnCloseRoom;
        public event OnOpenRoomDelegate OnOpenRoom;
        public event OnRoomClearedDelegate OnRoomCleared;

        public LevelManager LevelManager => _levelManager;
        public RoomCell[] Cells => _cells;
        public Color colorCoding;
        public Sprite Icon => _icon;
        public Vector2Int GridPosition => _gridposition;
        #endregion

        #region Private Fields
        [SerializeField] Sprite _icon;
        [SerializeField] RoomCell[] _cells;
        RoomState _roomState = RoomState.InActive;  
        LevelManager _levelManager;
        Vector2Int _gridposition;
        IRoomEvent[] _events;
        #endregion

        #region Public Methods
        public void Initialize(LevelManager manager)
        {
            _levelManager = manager;
            _events = GetComponentsInChildren<IRoomEvent>();

            foreach (RoomCell cell in _cells) {
                cell.Initialize(this);
            }

            foreach (IRoomEvent evt in _events) {
                evt.OnRoomInitialize(this);
            }
        }

        public void OnCreation(Vector2Int gridPosition)
        {
            _gridposition = gridPosition;
        }

        public void CloseRoom()
        {
            OnCloseRoom?.Invoke();
        }

        public void RoomCleared()
        {
            _roomState = RoomState.Cleared;
            OnRoomCleared?.Invoke();
            OnOpenRoom?.Invoke();
        }
        #endregion

        #region Private Methods
        void PlayerEnter()
        {
            if (_roomState != RoomState.InActive) { return; }
            _roomState = RoomState.Active;
            OnPlayerEnter?.Invoke();

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

            for (int i = 0; i < _cells.Length; i++) {
                Vector2 pos = (Vector2)transform.position + (_cells[i].Pos * GenerationUtilities.CellSize);

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
