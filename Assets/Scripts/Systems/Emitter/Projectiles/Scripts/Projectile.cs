using BulletHell.EffectInterfaces;
using BulletHell.StatusSystem;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

namespace BulletHell.Emitters.Projectiles
{
    public class Projectile : MonoBehaviour, IPoolable
    {
        #region Public Fields
        [HideInInspector] public ProjectileData Data;
        [HideInInspector] public bool hasCollision = true;
        public float LifeTime;
        public Vector3 Target;
        public Vector3 Velocity;
        public void SetPool(ObjectPool<Projectile> pool) => _pool = pool;
        #endregion

        #region Private Fields
        GameObject _owner;
        ObjectPool<Projectile> _pool;
        Vector3 _inheritedVelocity = Vector3.zero;
        #region Components
        SpriteRenderer _spriteRenderer;
        BoxCollider2D _collider;
        #endregion
        #endregion

        #region Private Methods
        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _collider = GetComponent<BoxCollider2D>();
        }

        IEnumerator ChangeColorOverLife()
        {
            float totalTime = LifeTime;

            yield return LerpColors(totalTime / 2, Data.MidLifeColor);
            yield return LerpColors(totalTime / 2, Data.DeathColor);
        }

        IEnumerator LerpColors(float time, Color target)
        {
            float timeElapsed = 0;
            Color startColor = _spriteRenderer.color;

            while (timeElapsed < time) {
                yield return new WaitForEndOfFrame();
                timeElapsed += Time.deltaTime;
                _spriteRenderer.color = Color.Lerp(startColor, target, timeElapsed / time);
            }

            _spriteRenderer.color = target;
        }

        private void FixedUpdate()
        {
            foreach (BaseProjectileBehaviour behaviour in Data.Behaviours) {
                behaviour.UpdateBehaviour(this, Data, Time.fixedDeltaTime);
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
            if(Data.MaxSpeed != 0)
                Velocity = Vector2.ClampMagnitude(Velocity, Data.MaxSpeed);

            Vector3 i_v = _inheritedVelocity * Mathf.Clamp01(Vector3.Dot(_inheritedVelocity.normalized, Velocity)) * Time.fixedDeltaTime;

            transform.position += new Vector3(Velocity.x * Time.fixedDeltaTime, Velocity.y * Time.fixedDeltaTime + Velocity.z * Time.fixedDeltaTime) + i_v;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(!hasCollision) { return; }
            CheckCollision(collision);
        }
        #endregion

        #region Public Methods
        public void Initialize(ProjectileData data, GameObject owner)
        {
            Data = data;
            _owner = owner;
            if (Data.InheritVelocity && _owner.TryGetComponent(out Rigidbody2D rb))
                _inheritedVelocity = (Vector3)rb.velocity;


            _spriteRenderer.sprite = Data.Sprite;

            _collider.offset = Data.Collider.center;
            _collider.size = Data.Collider.size / 2;

            transform.localScale = Vector3.one * Data.Scale;
            _spriteRenderer.color = Data.BirthColor;

            foreach (BaseProjectileBehaviour behaviour in Data.Behaviours) {
                behaviour.Initialize(this, Data);
            }

            if (Data.BirthVFX != null)
                PlayVFX(Data.BirthVFX, true);

            StartCoroutine(ChangeColorOverLife());
        }

        public void CheckCollision(Collider2D[] colliders)
        {
            foreach (Collider2D collider in colliders) {
                CheckCollision(collider);
            }
        }

        public void CheckCollision(Collider2D collision) {
            if (collision == null) { return; }
            if (!Data.CollisionTags.Contains(collision.tag)) { return; }
            if(collision.TryGetComponent(out IDamageable entity)) {
                entity.Damage(Data.Damage);
            }
            if(collision.TryGetComponent(out UnitStatusEffects effectContainer)) {
                foreach (StatusEffect effect in Data.StatusEffects) {
                    effectContainer.ApplyEffect(effect);
                }
            }

            if (Data.DestroyOnCollision) ResetObject();
        }

        public void PlayVFX(VisualEffectAsset asset, bool playInWorldSpace = true)
        {
            Transform parent = playInWorldSpace ? null : transform;
            BulletHell.VFX.VFXManager.PlayBurst(asset, transform.position, parent);
        }

        public void ResetObject()
        {
            StopAllCoroutines();

            if(Data.DeathVFX != null) 
                PlayVFX(Data.DeathVFX, true);

            gameObject.SetActive(false);

            _inheritedVelocity = Vector3.zero;
            transform.position = Vector2.zero;
            Velocity = Vector2.zero;
            
            Data = null;
            
            _pool.Release(this);
        }
        #endregion
    }
}