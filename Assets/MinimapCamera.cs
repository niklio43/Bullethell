using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float _mipmapZoom;

    void Update()
    {
        transform.position = new Vector3(target.position.x, target.position.y, _mipmapZoom);
    }
}
