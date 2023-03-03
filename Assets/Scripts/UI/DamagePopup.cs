using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using BulletHell;

public class DamagePopup : MonoBehaviour, IPoolable
{
    TextMeshPro _textMesh;
    float _disappearTimer;
    Color _textColor, _savedColor;
    Vector3 _moveVector;

    const float DISAPPEAR_TIMER_MAX = 1f;

    static int sortingOrder;

    public ObjectPool<DamagePopup> Pool;

    Vector2 _startScale;

    void Awake()
    {
        _textMesh = GetComponent<TextMeshPro>();
        _startScale = transform.localScale;
    }

    public void ResetObject()
    {
        _textMesh.color = _savedColor;
        transform.localScale = _startScale;

        gameObject.SetActive(false);
        Pool.Release(this);
    }

    public void Setup(float damageAmount, Vector2 startPos)
    {
        transform.position = startPos;
        _savedColor = _textMesh.color;

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

        if (_disappearTimer > DISAPPEAR_TIMER_MAX * .5f)
            transform.localScale += Vector3.one * 1f * Time.deltaTime;
        else
            transform.localScale -= Vector3.one * 1f * Time.deltaTime;

        _disappearTimer -= Time.deltaTime;
        if (_disappearTimer <= 0)
        {
            _textColor.a -= 3f * Time.deltaTime;
            _textMesh.color = _textColor;
            if (_textColor.a <= 0)
            {
                ResetObject();
            }
        }
    }
}
