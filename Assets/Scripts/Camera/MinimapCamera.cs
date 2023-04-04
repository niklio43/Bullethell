using BulletHell.Map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell;

public class MinimapCamera : MonoBehaviour
{
    #region Private Fields
    Camera _cam;
    #endregion

    #region Public Fields
    public Camera Cam => _cam;
    #endregion

    #region Private Methods
    void Start()
    {
        _cam = GetComponent<Camera>();
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
    #endregion

    #region Public Methods
    public void SetPosition(Vector2 pos)
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
    #endregion

}
