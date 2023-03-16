using System.Collections.Generic;
using UnityEngine;
using BulletHell.Map.Generation;

namespace BulletHell.Map
{
    public class Map : MonoBehaviour
    {
        List<SpriteRenderer> _roomIcons = new List<SpriteRenderer>();
        LevelManager _levelManager;

        public void Initialize(LevelManager manager)
        {
            _levelManager = manager;
        }

        public void CreateMap()
        {
            List<Room> rooms = _levelManager.Rooms;

            foreach (Room room in rooms) {
                SpriteRenderer roomIcon = new GameObject().AddComponent<SpriteRenderer>();
                roomIcon.transform.parent = transform;
                roomIcon.name = $"{room.name} (Icon)";
                roomIcon.sprite = room.Icon;

                roomIcon.transform.localPosition = (Vector2)(room.GridPosition);
            }
        }
    }
}
