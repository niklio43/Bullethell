using BulletHell.Stats;
using BulletHell.VFX;
using UnityEngine;

namespace BulletHell.Emitters.Projectiles
{
    public class EmitterProjectile : MonoBehaviour, IPoolable
    {
        Character _owner;

        public Pool<EmitterProjectile> Pool;
        EmittterProjectileData _data;

        Animator _anim;
        SpriteRenderer _spriteRenderer;
        RuntimeEmitterProjectileData _runTimeData;
        BoxCollider2D _projectileCollider;
        DamageInfo _damage;

        public float TimeToLive => _runTimeData.TimeToLive;


        private void Awake()
        {
            _anim = GetComponent<Animator>();
            _projectileCollider = GetComponent<BoxCollider2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _runTimeData = new RuntimeEmitterProjectileData();
        }

        public void Initialize(EmittterProjectileData data, RuntimeEmitterProjectileData runTimeData)
        {
            _data = data;
            gameObject.SetActive(true);
            if (data.Sprite != null)
                _spriteRenderer.sprite = data.Sprite;

            _projectileCollider.offset = _data.collider.center;
            _projectileCollider.size = _data.collider.size / 2;

            transform.localScale = Vector3.one * data.Scale;
            _spriteRenderer.color = data.Color;

            if (data.AnimationClip == null)
                _anim.enabled = false;
            else {
                _anim.enabled = true;
                _anim.Play(data.AnimationClip.name);
            }

            _runTimeData = runTimeData;
            _runTimeData.SetOwner(this);
            _runTimeData.UpdateData(Time.fixedDeltaTime);
        }

        public void SetOwner(Character owner) => _owner = owner;
        public void SetDamage(DamageInfo damage) => _damage = damage;

        private void FixedUpdate()
        {
            _runTimeData.UpdateData(Time.fixedDeltaTime);
        }

        public void ResetObject()
        {
            _data = null;
            transform.position = Vector3.zero;
            gameObject.SetActive(false);
            Pool.Release(this);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!_data.CollisionTags.Contains(collision.gameObject.tag) || _damage == null || collision.gameObject == _owner) { return; }
            if (collision.TryGetComponent(out Character character)) {
            }
            OnHit();
        }

        void OnHit()
        {
            if (_data.HitVFX != null)
                VFXManager.PlayBurst(_data.HitVFX, transform.position);

            Destroy(gameObject);
        }
    }
}
