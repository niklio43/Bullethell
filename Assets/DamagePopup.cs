using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    TextMeshPro _textMesh;
    float _disappearTimer;
    Color _textColor;
    Vector3 _moveVector;

    const float DISAPPEAR_TIMER_MAX = 1f;

    static int sortingOrder;

    void Awake()
    {
        _textMesh = GetComponent<TextMeshPro>();
    }
    public void Setup(float damageAmount)
    {
        _textMesh.SetText(damageAmount.ToString());

        _disappearTimer = DISAPPEAR_TIMER_MAX;

        sortingOrder++;
        _textMesh.sortingOrder = sortingOrder;
        _moveVector = new Vector3(.7f, 1) * 10f;
    }

    void Update()
    {
        transform.position += _moveVector * Time.deltaTime;
        _moveVector -= _moveVector * 8f * Time.deltaTime;

        if(_disappearTimer > DISAPPEAR_TIMER_MAX * .5f)
        {
            float increaseScaleAmount = 1f;
            transform.localScale += Vector3.one * increaseScaleAmount * Time.deltaTime;
        }
        else
        {
            float decreaseScaleAmount = 1f;
            transform.localScale -= Vector3.one * decreaseScaleAmount * Time.deltaTime;
        }

        _disappearTimer -= Time.deltaTime;
        if(_disappearTimer <= 0)
        {
            float disappearSpeed = 3f;
            _textColor.a -= disappearSpeed * Time.deltaTime;
            _textMesh.color = _textColor;
            if(_textColor.a <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

}
