using BulletHell.Stats;
using BulletHell.StatusSystem;
using UnityEngine;
using UnityEngine.VFX;

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

        Vector3 _inheritedVelocity = Vector3.zero;

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

            if (_data.InheritVelocity && _owner.TryGetComponent(out Rigidbody2D rb))
                _inheritedVelocity = (Vector3)rb.velocity;


            _spriteRenderer.sprite = _data.Sprite;

            _collider.offset = _data.Collider.center;
            _collider.size = _data.Collider.size / 2;

            transform.localScale = Vector3.one * _data.Scale;
            _spriteRenderer.color = _data.BirthColor;

            foreach (BaseProjectileBehaviour behaviour in _data.Behaviours) {
                behaviour.Initialize(this, _data);
            }

            if (_data.BirthVFX != null)
                PlayVFX(_data.BirthVFX, true);

            InitializeDamage();
        }

        void InitializeDamage()
        {
            foreach (StatusEffect statusEffect in _data.StatusEffects) {
                statusEffect.Initialize(_owner);
            }
             
            Damage = new DamageInfo(_data.Damage, _data.StatusEffects);
            Damage = DamageHandler.CalculateDamage(Damage, _owner.Stats);
        }

        private void FixedUpdate()
        {
            foreach (BaseProjectileBehaviour behaviour in _data.Behaviours) {
                behaviour.UpdateBehaviour(this, _data, Time.fixedDeltaTime);
            }

            LifeTime -= Time.fixedDeltaTime;

            UpdatePosition();
            
            if (LifeTime <= 0)
                ResetObject();
        }

        void UpdatePosition()
        {
            float angle = Mathf.Atan2(Velocity.y, Velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
            if(_data.MaxSpeed != 0)
                Velocity = Vector2.ClampMagnitude(Velocity, _data.MaxSpeed);

            Vector3 i_v = _inheritedVelocity * Mathf.Clamp01(Vector3.Dot(_inheritedVelocity.normalized, Velocity)) * Time.fixedDeltaTime;

            transform.position += new Vector3(Velocity.x * Time.fixedDeltaTime, Velocity.y * Time.fixedDeltaTime + Velocity.z * Time.fixedDeltaTime) + i_v;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(!hasCollision) { return; }
            CheckCollision(collision);
        }

        public void CheckCollision(Collider2D[] colliders)
        {
            foreach (Collider2D collider in colliders) {
                CheckCollision(collider);
            }
        }

        public void CheckCollision(Collider2D collision) {
            if (collision == null) { return; }
            if (!_data.CollisionTags.Contains(collision.tag)) { return; }
            if (collision.gameObject.TryGetComponent(out Character receiver)) {
                DealDamage(receiver);
            }
            if (_data.DestroyOnCollision) ResetObject();
        }

        public void DealDamage(Character receiver)
        {
            DamageHandler.Send(_owner, receiver, Damage);
        }

        public void PlayVFX(VisualEffectAsset asset, bool playInWorldSpace = true)
        {
            if(asset == null) { return; }
            Transform parent = playInWorldSpace ? null : transform;
            BulletHell.VFX.VFXManager.PlayBurst(asset, transform.position, parent);
        }

        public void ResetObject()
        {
            PlayVFX(_data.DeathVFX, true);

            gameObject.SetActive(false);

            _inheritedVelocity = Vector3.zero;
            transform.position = Vector2.zero;
            Velocity = Vector2.zero;
            
            Damage = null;
            _data = null;
            
            _pool.Release(this);
        }
    }
}