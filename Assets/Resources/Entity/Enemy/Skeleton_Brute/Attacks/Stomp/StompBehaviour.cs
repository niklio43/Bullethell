using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Abilities;
using UnityEngine.VFX;
using BulletHell.StatusSystem;
using BulletHell.Emitters;
using BulletHell.Stats;
using BulletHell.Emitters.Projectiles;

namespace BulletHell.Enemies.AbilityBehaviours
{
    [CreateAssetMenu(fileName = "EnemyStompBehaviour", menuName = "Abilities/Custom Behaviours/New Enemy Stomp Behaviour")]
    public class StompBehaviour : EmitterAbilityBehaviour
    {
        [Header("Damage")]
        [SerializeField] List<DamageValue> Damage;
        [SerializeField] List<StatusEffect> StatusEffects;
        [Header("Other")]
        [SerializeField] float timeBetween = .1f;
        [SerializeField] float timeForDetonation = 1f;
        [SerializeField] VisualEffectAsset VFX;
        [Header("Collision")]
        [SerializeField] List<string> _collisionTags;


        float timeTilNextZone = 0;
        DamageInfo _damage;
        Projectile _projectile;

        protected override void Initialize()
        {
            _emitterObject = new Emitter(_ability.Host, _emitterData);

            Character character = _ability.Owner.GetComponent<Character>();

            foreach (StatusEffect statusEffect in StatusEffects) {
                statusEffect.Initialize(character);
            }

            _damage = new DamageInfo(Damage, StatusEffects);
            _damage = DamageHandler.CalculateDamage(_damage, character.Stats);
        }

        protected override void OnUpdate(float dt)
        {
            if(_projectile == null) { return; } 
            if(!_projectile.gameObject.activeSelf) { _projectile = null; return; }

            if(timeTilNextZone < 0) {
                _projectile.StartCoroutine(PlaceAndDetonateZone());
            }
            timeTilNextZone -= dt;
        }

        IEnumerator PlaceAndDetonateZone()
        {
            float timeElapsed = 0;
            DamageZone zone = DamageZoneManager.PlaceZone(_projectile.transform.position, timeElapsed);

            while (timeElapsed < timeForDetonation) {
                yield return new WaitForEndOfFrame();
                timeElapsed += Time.deltaTime;
            }

            CheckCollision(zone.Activate());
        }

        void CheckCollision(Collider2D[] colliders)
        {
            foreach (Collider2D collider in colliders) {
                CheckCollision(collider);
            }
        }

        public void CheckCollision(Collider2D collision)
        {
            if (collision == null || !_collisionTags.Contains(collision.tag)) { return; }
            if (collision.gameObject.TryGetComponent(out Character receiver)) {
                DealDamage(receiver);
            }
        }

        void DealDamage(Character receiver)
        {
            Character sender = _ability.Owner.GetComponent<Character>();
            DamageHandler.Send(sender, receiver, _damage);
        }

        protected override void Perform()
        {
            Debug.Log("STOMP!");

            if (_ability.Owner.TryGetComponent(out Character character)) {
                _projectile = _emitterObject.FireProjectile(character, Target)[0];
                timeTilNextZone = timeBetween;
            }
            else {
                Debug.LogError("Ability owner needs to be of type Character!");
            }
        }


    }
}
