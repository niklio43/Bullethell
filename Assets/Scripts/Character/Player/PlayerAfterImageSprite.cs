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

    Transform _player;
    SpriteRenderer _sr;
    SpriteRenderer _playerSr;
    Color _color;

    PlayerController _playerController;

    public ObjectPool<PlayerAfterImageSprite> Pool;

    public void ResetObject()
    {
        gameObject.SetActive(false);
        Pool.Release(this);
    }

    public void Initialize(float offset)
    {
        _sr = GetComponent<SpriteRenderer>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _playerSr = _player.GetComponent<SpriteRenderer>();
        _playerController = _player.GetComponent<PlayerController>();
        Vector2 dir = _playerController.MovementInput.normalized;

        _alphaMultiplier = _alphaMultiplierStart;
        _alphaMultiplier += offset / 100;
        Mathf.Clamp(_alphaMultiplier, 0, 1);

        _alpha = _alphaSet;
        _sr.sprite = _playerSr.sprite;
        transform.position = new Vector2
            (_player.position.x + (dir.x * offset), _player.position.y + (dir.y * offset));
        transform.rotation = _player.rotation;
        transform.localScale = _player.localScale;
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
