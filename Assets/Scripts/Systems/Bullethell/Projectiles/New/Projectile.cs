using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.Emitters.Projectiles
{
    public class Projectile : MonoBehaviour, IPoolable
    {
        ProjectileData _data;
        Character _owner;

        ObjectPool<Projectile> _pool;

        #region Setters
        public void SetOwner(Character owner) => _owner = owner;
        public void SetPool(ObjectPool<Projectile> pool) => _pool = pool;
        #endregion

        #region Getters
        public Character GetOwner() => _owner;
        #endregion

        public Transform Target;
        public float LifeTime;
        public Vector2 Velocity;

        #region Components
        SpriteRenderer _spriteRenderer;
        BoxCollider2D _collider;
        #endregion

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _collider = GetComponent<BoxCollider2D>();
        }

        public void Initialize(ProjectileData data)
        {
            _data = data;

            if(data.Sprite != null)
                _spriteRenderer.sprite = data.Sprite;

            _collider.offset = _data.Collider.center;
            _collider.size = _data.Collider.size / 2;

            transform.localScale = Vector3.one * _data.Scale;
            _spriteRenderer.color = _data.Birth;

            //TODO implement color over life!!

            foreach (BaseProjectileBehaviour behaviour in _data.Behaviours) {
                behaviour.Initialize(this, _data);
            }
        }

        private void FixedUpdate()
        {
            foreach (BaseProjectileBehaviour behaviour in _data.Behaviours) {
                behaviour.UpdateBehaviour(this, _data, Time.fixedDeltaTime);
            }

            Velocity = Vector2.ClampMagnitude(Velocity, _data.MaxSpeed);
            LifeTime -= Time.fixedDeltaTime;

            transform.position += (Vector3)Velocity * Time.fixedDeltaTime;

            if (LifeTime <= 0)
                ResetObject();
        }

        public void ResetObject()
        {
            gameObject.SetActive(false);
            transform.position = Vector2.zero;
            Velocity = Vector2.zero;
            _data = null;
            _pool.Release(this);
        }
    }
}