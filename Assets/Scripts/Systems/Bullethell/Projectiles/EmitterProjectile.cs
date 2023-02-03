using BulletHell.Stats;
using BulletHell.StatusSystem;
using BulletHell.VFX;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.Emitters.Projectiles
{
    public class EmitterProjectile : MonoBehaviour, IPoolable
    {
        Character _owner;

        public Pool<EmitterProjectile> Pool;
        public EmittterProjectileData Data;

        Animator _anim;
        SpriteRenderer _spriteRenderer;
        RuntimeEmitterProjectileData _runTimeData;
        BoxCollider2D _projectileCollider;
        DamageInfo _damage;
        List<StatusEffect> _statusEffects = new List<StatusEffect>();

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
            Data = data;
            gameObject.SetActive(true);
            if (data.Sprite != null)
                _spriteRenderer.sprite = data.Sprite;

            _projectileCollider.offset = Data.collider.center;
            _projectileCollider.size = Data.collider.size / 2;

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
        public void SetStatusEffect(List<StatusEffect> effects) => _statusEffects = effects;

        private void FixedUpdate()
        {
            _runTimeData.UpdateData(Time.fixedDeltaTime);
        }

        public void ResetObject()
        {
            Data = null;
            transform.position = Vector3.zero;
            gameObject.SetActive(false);
            Pool.Release(this);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!Data.CollisionTags.Contains(collision.gameObject.tag) || _damage == null || collision.gameObject == _owner) { return; }
            if (collision.TryGetComponent(out Character character)) {
                if (_damage != null)
                    DamageHandler.SendDamage(_owner, character, _damage);

                if (_statusEffects != null)
                    foreach (StatusEffect effect in _statusEffects) {
                        effect.ApplyEffect(character);
                    }
            }
            OnHit();
        }

        void OnHit()
        {
            if (Data.HitVFX != null)
                VFXManager.PlayBurst(Data.HitVFX, transform.position);

            Destroy(gameObject);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _runTimeData.FollowRange);
        }
    }
}
