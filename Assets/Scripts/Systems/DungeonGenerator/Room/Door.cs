using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.Map
{
    public class Door : MonoBehaviour
    {
        [SerializeField] Direction Orientation;
        [SerializeField] Vector2Int Cell;

        [SerializeField] GameObject Open;
        [SerializeField] GameObject Blocked;

        [SerializeField] Door connectedDoor;

        public bool IsConnected() => connectedDoor != null;

        Room _room;


        public void Initialize(Room room)
        {
            _room = room;
        }

        private void Awake()
        {
            BlockDoor();
        }

        public void OpenDoor()
        {
            Open.SetActive(true);
            Blocked.SetActive(false);
        }

        public void BlockDoor()
        {
            Open.SetActive(false);
            Blocked.SetActive(true);
        }

        public Vector2Int GetCellPosition() => Cell;

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Vector2 pos = (Vector2)transform.position + (Cell * 20);

            Gizmos.DrawWireCube(pos, Vector3.one * 20);
        }

    }
}
