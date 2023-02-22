using BulletHell.Stats;
using UnityEngine;

namespace BulletHell.Emitters.Projectiles
{
    public class Projectile : MonoBehaviour, IPoolable
    {
        ProjectileData _data;
        Character _owner;

        ObjectPool<Projectile> _pool;

        public DamageInfo Damage { get; private set; }
        public Transform Target;
        public float LifeTime;
        public Vector3 Velocity;

        [HideInInspector] public bool hasCollision = true;

        #region Setters
        public void SetOwner(Character owner) => _owner = owner;
        public void SetPool(ObjectPool<Projectile> pool) => _pool = pool;
        #endregion

        #region Getters
        public Character GetOwner() => _owner;
        public ObjectPool<Projectile> GetPool() => _pool;
        #endregion

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

            if (_data.Sprite != null)
                _spriteRenderer.sprite = _data.Sprite;

            _collider.offset = _data.Collider.center;
            _collider.size = _data.Collider.size / 2;

            transform.localScale = Vector3.one * _data.Scale;

            Damage = new DamageInfo(_data.Damage, _data.StatusEffects);
            Damage = DamageHandler.CalculateDamage(Damage, _owner.Stats);

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

            LifeTime -= Time.fixedDeltaTime;

            transform.position += new Vector3(Velocity.x * Time.fixedDeltaTime, Velocity.y * Time.fixedDeltaTime + Velocity.z * Time.fixedDeltaTime);

            if (LifeTime <= 0)
                ResetObject();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!hasCollision) { return; }
            DealDamage(collision);
            if (_data.DestroyOnCollision) ResetObject();
        }

        public void DealDamage(Collider2D[] colliders)
        {
            foreach (Collider2D collider in colliders) {
                DealDamage(collider);
            }
        }

        public void DealDamage(Collider2D collision)
        {
            if (!_data.CollisionTags.Contains(collision.tag)) { return; }
            if (!collision.gameObject.TryGetComponent(out Character receiver)) { return; }

            DamageHandler.Send(_owner, receiver, Damage);
        }

        public void ResetObject()
        {
            gameObject.SetActive(false);
            transform.position = Vector2.zero;
            Velocity = Vector2.zero;
            Damage = null;
            _data = null;
            _pool.Release(this);
        }
    }
}