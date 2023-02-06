using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell;
using BulletHell.Player;

public class PlayerAfterImageSprite : MonoBehaviour, IPoolable
{
    [SerializeField] float _activeTime = .1f;
    float _timeActivated;
    float _alpha;
    float _alphaSet = .8f;
    [SerializeField] float _alphaMultiplierStart = .9f;
    float _alphaMultiplier;

    SpriteRenderer _sr;
    Color _color;

    public ObjectPool<PlayerAfterImageSprite> Pool;

    void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
    }

    public void ResetObject()
    {
        gameObject.SetActive(false);
        Pool.Release(this);
    }

    public void Initialize(Transform player, Sprite sprite)
    {
        _alphaMultiplier = _alphaMultiplierStart;
        _alphaMultiplier += 2 / 100;
        Mathf.Clamp(_alphaMultiplier, 0, 1);

        _alpha = _alphaSet;
        _sr.sprite = sprite;
        transform.position = player.position;
        transform.rotation = player.rotation;
        transform.localScale = player.localScale;
        _timeActivated = Time.time;
    }

    void Update()
    {
        _alpha *= _alphaMultiplier;
        _color = new Color(1f, 1f, 1f, _alpha);
        _sr.color = _color;

        if(Time.time >= (_timeActivated + _activeTime))
        {
            ResetObject();
        }
    }
}
