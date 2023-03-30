using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BulletHell.Emitters.Projectiles.Behaviours
{
    //[CreateAssetMenu(fileName = "AirborneBehaviour", menuName = "Emitters/ProjectileBehaviours/Airborne")]
    public class AirborneBehaviour : BaseProjectileBehaviour
    {
        #region Private Fields
        [SerializeField] float _gravity = 9.8f;
        [SerializeField] float _impactSize = 2;
        [SerializeField] float _offset = 1;
        [SerializeField] float _maxDistance = 1;
        [SerializeField, Range(.1f, 10)] float flightDuration;
        #endregion

        #region Public Fields
        public override string Id() => "04";
        #endregion

        #region Public Methods
        public override void Initialize(Projectile owner, ProjectileData data)
        {
            Vector2 dir = owner.Target - owner.transform.position;
            float dist = Mathf.Clamp(Vector2.Distance(owner.Target, owner.transform.position), 0, _maxDistance);
            Vector2 offset = Random.insideUnitCircle * _offset;

            Vector2 target = (Vector2)owner.transform.position + (dir * dist + offset);

            owner.StartCoroutine(ArcRoutine(target, owner, data));
        }
        #endregion

        #region Private Methods
        IEnumerator ArcRoutine(Vector2 target, Projectile owner, ProjectileData data)
        {
            Vector2 dir = target - (Vector2)owner.transform.position;
            Vector2 normalizedDir = dir.normalized;

            float dist = Vector2.Distance(owner.transform.position, target);
            float speed = dist / flightDuration;

            DamageZone zone = DamageZoneManager.PlaceZone(target, _impactSize);

            float initalVerticalVelocity = flightDuration * (_gravity / 2);
            Vector3 velocity = new Vector3(normalizedDir.x * speed, normalizedDir.y * speed, initalVerticalVelocity);

            float timeElapsed = 0;
            while (timeElapsed < flightDuration) {
                yield return new WaitForFixedUpdate();
                float dt = Time.fixedDeltaTime;
                timeElapsed += dt;

                velocity.z -= _gravity * dt;
                owner.Velocity = velocity;
            }

            owner.CheckCollision(zone.Activate());
            owner.ResetObject();
        }
        #endregion
    }
}
