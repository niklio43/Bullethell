using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.Map
{
    public class Door : MonoBehaviour
    {
        [SerializeField] Direction _orientation;
        
        [SerializeField] GameObject Open;
        [SerializeField] GameObject Blocked;

        [SerializeField] Door connectedDoor;

        #region Getters & Setters
        public Direction GetOrientation() => _orientation;
        public bool IsConnected() => connectedDoor != null;

        #endregion

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
            connectedDoor = door;
            Open.SetActive(true);
            Blocked.SetActive(false);
        }

        public void BlockDoor()
        {
            Open.SetActive(false);
            Blocked.SetActive(true);
        }
    }
}
