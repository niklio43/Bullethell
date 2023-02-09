using BulletHell.Stats;
using BulletHell.StatusSystem;
using BulletHell.VFX;
using System.Collections;
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
            _spriteRenderer.color = data.birth;

            if (data.AnimationClip == null)
                _anim.enabled = false;
            else {
                _anim.enabled = true;
                _anim.Play(data.AnimationClip.name);
            }

            _runTimeData = runTimeData;
            _runTimeData.SetOwner(this);
            StartCoroutine(ChangeColorOverLife());
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
            StopAllCoroutines();
            transform.position = Vector3.zero;
            gameObject.SetActive(false);
            Pool.Release(this);
        }

        IEnumerator ChangeColorOverLife()
        {
            float totalTime = _runTimeData.TimeToLive;

            yield return LerpColors(totalTime/2, Data.midLife);
            yield return LerpColors(totalTime / 2, Data.Death);
        }


        IEnumerator LerpColors(float time, Color target)
        {
            float timeElapsed = 0;
            Color startColor = _spriteRenderer.color;

            while(timeElapsed < time) {
                yield return new WaitForEndOfFrame();
                timeElapsed += Time.deltaTime;

                _spriteRenderer.color = Color.Lerp(startColor, target, timeElapsed / time); 
            }

            _spriteRenderer.color = target;
        }



        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!Data.CollisionTags.Contains(collision.gameObject.tag) || _damage == null || collision.gameObject == _owner) { return; }
            if (collision.TryGetComponent(out Character character)) {
                if (_damage != null)
                    DamageHandler.Send(_owner, character, _damage);
            }
            OnHit();
        }

        void OnHit()
        {
            if (Data.HitVFX != null)
                VFXManager.PlayBurst(Data.HitVFX, transform.position);

            if(Data.DestroyOnHit)
                ResetObject();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _runTimeData.FollowRange);
        }
    }
}
