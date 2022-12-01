using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimWeapon : MonoBehaviour
{
    Transform _aimTransform;
    [SerializeField] float _aimLength = 1f;

    void Awake()
    {
        _aimTransform = transform.GetChild(0);
    }

    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 aimDirection = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        _aimTransform.eulerAngles = new Vector3(0, 0, angle);

        Vector3 shoulderToMouseDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - _aimTransform.position;
        shoulderToMouseDir.z = 0;
        _aimTransform.GetChild(0).position = _aimTransform.position + (_aimLength * shoulderToMouseDir.normalized);
    }
}