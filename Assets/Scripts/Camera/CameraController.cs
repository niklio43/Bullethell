using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Camera _camera;
    [SerializeField] Transform _target;
    [SerializeField] float _threshold;
    [SerializeField, Range(-20, 0)] float _zoom;
    [SerializeField, Range(0, 0.3f)] float _damping;
    void Update()
    {
        Vector3 mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 targetPos = (_target.position + mousePos) / 2f;

        targetPos.x = Mathf.Clamp(targetPos.x, -_threshold + _target.position.x, _threshold + _target.position.x);
        targetPos.y = Mathf.Clamp(targetPos.y, -_threshold + _target.position.y, _threshold + _target.position.y);

        transform.position = Vector3.Lerp(transform.position, new Vector3(targetPos.x, targetPos.y, _zoom), _damping);
    }
}