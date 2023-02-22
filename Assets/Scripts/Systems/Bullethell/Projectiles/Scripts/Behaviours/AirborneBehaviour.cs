using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BulletHell.Emitters.Projectiles.Behaviours
{
    //[CreateAssetMenu(fileName = "AirborneBehaviour", menuName = "Emitters/ProjectileBehaviours/Airborne")]
    public class AirborneBehaviour : BaseProjectileBehaviour
    {
        [SerializeField] float gravity = 9.8f;
        [SerializeField] float impactSize = 2;
        [SerializeField] float offset = 1;
        [SerializeField, Range(.1f, 10)] float flightDuration;
        public override string Id() => "04";

        public override void Initialize(Projectile owner, ProjectileData data)
        {
            Vector2 target = (Vector2)owner.Target.position + Random.insideUnitCircle * offset;
            owner.StartCoroutine(ArcRoutine(target, owner, data));
        }

        IEnumerator ArcRoutine(Vector2 target, Projectile owner, ProjectileData data)
        {
            Vector2 dir = target - (Vector2)owner.transform.position;
            Vector2 normalizedDir = dir.normalized;

            float dist = Vector2.Distance(owner.transform.position, target);
            float speed = dist / flightDuration;

            DamageZone zone = DamageZoneManager.PlaceZone(target);
            zone.Execute(impactSize);

            float initalVerticalVelocity = flightDuration * (gravity / 2);
            Vector3 velocity = new Vector3(normalizedDir.x * speed, normalizedDir.y * speed, initalVerticalVelocity);

            float timeElapsed = 0;
            while (timeElapsed < flightDuration) {
                yield return new WaitForFixedUpdate();
                float dt = Time.fixedDeltaTime;
                timeElapsed += dt;

                velocity.z -= gravity * dt;
                owner.Velocity = velocity;
            }

            owner.DealDamage(zone.Activate());
            owner.ResetObject();
        }
    }
}
