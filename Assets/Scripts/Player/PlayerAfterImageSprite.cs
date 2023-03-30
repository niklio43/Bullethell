using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell;
using BulletHell.Player;

public class PlayerAfterImageSprite : MonoBehaviour, IPoolable
{
    #region Private Fields
    [SerializeField] float _activeTime = .1f;
    float _timeActivated;
    float _alpha;
    float _alphaSet = .8f;
    [SerializeField] float _alphaMultiplierStart = .9f;
    float _alphaMultiplier;

    SpriteRenderer _sr;
    Color _color;
    #endregion

    #region Public Fields
    public ObjectPool<PlayerAfterImageSprite> Pool;
    #endregion

    #region Private Methods
    void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
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
    #endregion

    #region Public Methods
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
    #endregion
}
