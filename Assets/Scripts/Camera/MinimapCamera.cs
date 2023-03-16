using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField, Range(0, 100)] float _mapZoom = 5;
    public float MapZoom { get { return _mapZoom; } set { _mapZoom = value; } }

    void Start()
    {
        
    }

    public void SetPosition(Vector2Int size)
    {
        Vector2 pos = new Vector2(size.x / 2, size.y / 2);
        transform.position = new Vector3(pos.x, pos.y, -100);
    }

    public void SetSize(float size)
    {
        _mapZoom = size;
        GetComponent<Camera>().orthographicSize = _mapZoom;
    }

    void Update()
    {
        transform.position = new Vector3(target.position.x, target.position.y, -100);
        GetComponent<Camera>().orthographicSize = _mapZoom;
    }
}
