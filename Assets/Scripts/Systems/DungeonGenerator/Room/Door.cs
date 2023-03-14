using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.Map
{
    public class Door : MonoBehaviour
    {
        [SerializeField] Direction _orientation;
        
        [SerializeField] GameObject _open;
        [SerializeField] GameObject _closed;
        [SerializeField] GameObject _blocker;

        Room _room;

        DoorState _state = DoorState.Open;
        public enum DoorState
        {
            Open,
            Closed
        }

        Door _connectedDoor;




        #region Getters & Setters
        public Direction GetOrientation() => _orientation;
        public bool IsConnected() => _connectedDoor != null;

        #endregion

        public void Initialize(Room room)
        {
            _room = room;
            room.OnPlayerEnter += BlockDoor;
            room.OnRoomCleared += UnBlockDoor;
        }

        public Direction GetConnecteeOrientation()
        {
            switch (_orientation) {
                case Direction.Up:
                    return Direction.Down;
                case Direction.Down:
                    return Direction.Up;
                case Direction.Left:
                    return Direction.Right;
                case Direction.Right:
                    return Direction.Left;
            }

            return Direction.Up;
        }

        public void OpenDoor(Door door)
        {
            _connectedDoor = door;

            _state = DoorState.Open;
            _open.SetActive(true);
            _closed.SetActive(false);
        }

        public void CloseDoor()
        {
            _state = DoorState.Closed;
            _open.SetActive(false);
            _closed.SetActive(true);
        }

        public void BlockDoor()
        {
            if(_state == DoorState.Closed) { return; }
            _blocker.SetActive(true);
        }

        public void UnBlockDoor()
        {
            _blocker.SetActive(false);
        }
    }
}
