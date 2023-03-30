using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Map;

public class Minimap : MonoBehaviour
{
    #region Private fields
    List<SpriteRenderer> _roomIcons = new List<SpriteRenderer>();
    MinimapCamera _camera;
    #endregion

    #region Public Methods
    public void OnGenerationFinished(Component sender, object data)
    {
        if (sender is not LevelManager) { return; }
        CreateMap(data as Room[]);
    }
    
    public void OnPlayerMoved(Component sender, object data)
    {
        if(sender is not LevelManager) { return; }
        UpdateCamera(data as Room);
    }

    public void UpdateCamera(Room room)
    {
        Debug.Log(_camera);
        _camera.SetPosition(room.GetCenterPosition());
    }
    #endregion

    #region Private Methods
    private void Awake()
    {
        _camera = GetComponentInChildren<MinimapCamera>();
    }
    private void CreateMap(Room[] rooms)
    {
        foreach (Room room in rooms) {
            SpriteRenderer roomIcon = new GameObject().AddComponent<SpriteRenderer>();
            roomIcon.transform.parent = transform;
            roomIcon.name = $"{room.name} (Icon)";
            roomIcon.sprite = room.Icon;

            roomIcon.transform.localPosition = (Vector2)(room.GridPosition);
        }
    }
    #endregion
}
