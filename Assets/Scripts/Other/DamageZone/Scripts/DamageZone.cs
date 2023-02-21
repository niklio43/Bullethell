using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell;

public class DamageZone : MonoBehaviour, IPoolable
{
    ObjectPool<DamageZone> _pool;
    SpriteRenderer _spriteRenderer;

    float _size = 0;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.material = Instantiate(_spriteRenderer.material);
    }

    public void Initialize(ObjectPool<DamageZone> pool)
    {
        _pool = pool;
    }

    public void Execute(float size)
    {
        _size = size;
        transform.localScale = Vector3.one * size;
    }

    public void Activate()
    {
        CheckDamage(_size);
        StartCoroutine(DamageRoutine());
    }

    void CheckDamage(float size)
    {
        StartCoroutine(DamageRoutine());
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, size, 1 << LayerMask.NameToLayer("Entity"));
        if(colliders.Length == 0) { return; }

        foreach (Collider2D collider in colliders) {
            if(collider.gameObject.TryGetComponent(out Character character)) {
                Debug.Log(character.name + " was in the zone when it executed!");
            }
        }
    }

    IEnumerator DamageRoutine()
    {
        float timeElapsed = 0;
        while (timeElapsed < .2f) {
            yield return new WaitForEndOfFrame();
            timeElapsed += Time.deltaTime;

            float ratio = Mathf.Clamp01(timeElapsed / .2f);

            float result = 1f/(1f + Mathf.Pow(2.7182f, -10f * (ratio - .5f)));

            _spriteRenderer.material.SetFloat("_maskScale", Mathf.Clamp01(result));
        }

        _spriteRenderer.material.SetFloat("_maskScale", 0);
        ResetObject();
    }

    public void ResetObject()
    {
        transform.position = Vector3.zero;
        gameObject.SetActive(false);
        _pool.Release(this);
    }
}
