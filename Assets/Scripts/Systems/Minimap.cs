using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Map;

public class Minimap : Singleton<Minimap>
{
    #region Private fields
    List<SpriteRenderer> _roomIcons = new List<SpriteRenderer>();
    #endregion

    #region Private Methods
    protected override void OnAwake()
    {
        LevelManager.OnInitialize += Initialize;
    }
    #endregion

    #region Public Methods
    public void Initialize()
    {
        CreateMap();
    }

    public void CreateMap()
    {
        List<Room> rooms = LevelManager.Instance.Rooms;

        foreach (Room room in rooms)
        {
            SpriteRenderer roomIcon = new GameObject().AddComponent<SpriteRenderer>();
            roomIcon.transform.parent = transform;
            roomIcon.name = $"{room.name} (Icon)";
            roomIcon.sprite = room.Icon;

            roomIcon.transform.localPosition = (Vector2)(room.GridPosition);
        }
    }
    #endregion
}
