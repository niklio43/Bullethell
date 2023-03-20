using BulletHell.Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell;

public class MinimapCamera : MonoBehaviour
{
    Camera _cam;
    public Camera Cam => _cam;

    void Start()
    {
        _cam = GetComponent<Camera>();
    }

    public void SetPosition(Vector2Int pos)
    {
        StartCoroutine(EasePosition(pos, 0.5f));
    }

    public void SetSize(float size)
    {
        _cam.orthographicSize = size;
    }

    public void SetTexture(RenderTexture texture)
    {
        _cam.targetTexture = texture;
    }

    IEnumerator EasePosition(Vector2 targetPos, float t)
    {
        Vector2 startPos = transform.position;
        float timeElapsed = 0;
        while (timeElapsed < t)
        {
            yield return new WaitForEndOfFrame();
            timeElapsed += Time.deltaTime;
            Vector2 newTarget;
            newTarget.x = Easing.EaseInOut(startPos.x, targetPos.x, timeElapsed / t);
            newTarget.y = Easing.EaseInOut(startPos.y, targetPos.y, timeElapsed / t);
            transform.position = new Vector3(newTarget.x, newTarget.y, -100);
        }
        transform.position = new Vector3(targetPos.x, targetPos.y, -100);
    }
}
