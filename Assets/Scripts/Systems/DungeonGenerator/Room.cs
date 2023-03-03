using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BulletHell.Map
{
    public class Room : MonoBehaviour
    {
        public Orientation Orientation;
        public GameObject[] Doors;
        
        BoxCollider2D _roomBounds;

        private void Awake()
        {
            _roomBounds = GetComponent<BoxCollider2D>();
        }


    }
}
