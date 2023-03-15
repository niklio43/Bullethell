using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField, Range(0, 100)] float _mapZoom = 5;
    public float MapZoom { get { return _mapZoom; } set { _mapZoom = value; } }

    void Update()
    {
        transform.position = new Vector3(target.position.x, target.position.y, -100);
        GetComponent<Camera>().orthographicSize = _mapZoom;
    }
}
