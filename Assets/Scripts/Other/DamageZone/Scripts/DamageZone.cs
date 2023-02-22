using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell;
using System;

public class DamageZone : MonoBehaviour, IPoolable
{
    ObjectPool<DamageZone> _pool;
    SpriteRenderer _spriteRenderer;

    float _size = 0;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.material = Instantiate(_spriteRenderer.material);

        transform.localScale = Vector3.zero;
    }

    public void Initialize(ObjectPool<DamageZone> pool)
    {
        _pool = pool;
    }

    public void Execute(float size)
    {
        _size = size;
        StartCoroutine(Animate(size, .35f));
    }

    public Collider2D[] Activate()
    {
        StartCoroutine(DamageRoutine());
        return CheckDamage(_size);
    }

    Collider2D[] CheckDamage(float size)
    {
        StartCoroutine(DamageRoutine());
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, size, 1 << LayerMask.NameToLayer("Entity"));
        return colliders;
    }

    IEnumerator Animate(float size, float duration)
    {
        float timeElapsed = 0;
        while(timeElapsed < duration) {
            yield return new WaitForEndOfFrame();
            timeElapsed += Time.deltaTime;

            transform.localScale = Vector3.one * Easing.EaseOutBack(0, size, timeElapsed / duration);
        }
        transform.localScale = Vector3.one * size;
    }

    IEnumerator DamageRoutine()
    {
        float timeElapsed = 0;
        while (timeElapsed < .2f) {
            yield return new WaitForEndOfFrame();
            timeElapsed += Time.deltaTime;

            float ratio = Mathf.Clamp01(timeElapsed / .2f);
            
            float result = Easing.EaseInOut(0, 1, ratio);
            transform.localScale = Vector3.one * Easing.EaseOutBack(_size, _size * 1.2f, ratio);
            _spriteRenderer.material.SetFloat("_maskScale", result);
        }

        _spriteRenderer.material.SetFloat("_maskScale", 0);
        ResetObject();
    }

    public void ResetObject()
    {
        transform.localScale = Vector3.zero;
        transform.position = Vector3.zero;
        gameObject.SetActive(false);
        _pool.Release(this);
    }
}
