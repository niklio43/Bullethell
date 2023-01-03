using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAimWeapon : MonoBehaviour
{
    [SerializeField] float _aimLength = 1f;

    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 aimDirection = (mousePosition - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, angle);

        Vector3 shoulderToMouseDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        shoulderToMouseDir.z = 0;
        transform.GetChild(0).position = transform.position + (_aimLength * shoulderToMouseDir.normalized);

        Vector2 scale = transform.localScale;
        if(aimDirection.x < 0)
        {
            scale.y = -1;
        }else if(aimDirection.x > 0)
        {
            scale.y = 1;
        }
        transform.localScale = scale;

        if(transform.eulerAngles.z > 50 && transform.eulerAngles.z < 120)
        {
            GetComponentInChildren<SpriteRenderer>().sortingOrder = 9;
        }
        else
        {
            GetComponentInChildren<SpriteRenderer>().sortingOrder = 11;
        }
    }
}
