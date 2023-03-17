using BulletHell.Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : Singleton<MinimapCamera>
{
    Camera _cam;
    public Camera Cam => _cam;

    void Start()
    {
        _cam = GetComponent<Camera>();
    }

    public void Initialize(Vector3 pos)
    {
        transform.position = new Vector3(pos.x, pos.y, -100);
    }

    public void SetSize(float size)
    {
        _cam.orthographicSize = size;
    }

    public void SetTexture(RenderTexture texture)
    {
        _cam.targetTexture = texture;
    }
}
