using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell;

namespace BulletHell.Emitters.Projectiles
{
    public class RuntimeEmitterProjectileData
    {
        EmitterProjectile _owner;
        public GameObject Target;
        public Vector2 Position;
        public Vector2 Velocity;
        public Vector2 Direction;
        public Vector2 GravityPoint;
        public float Gravity;
        public float TimeToLive;
        public float Acceleration;
        public float Speed;
        public bool FollowTarget;
        public float FollowIntensity;
        public float FollowRange;
        public float MaxSpeed;
        

        public void SetOwner(EmitterProjectile owner) => _owner = owner;

        public void UpdateData(float dt)
        {
            Velocity *= (1 + Acceleration * dt);
            Velocity = Vector2.ClampMagnitude(Velocity, MaxSpeed);

            if(FollowTarget) {
                CheckForTargets();
                Homeing(dt);
            }
            else {
                Vector2 gravity = (GravityPoint - Position).normalized * Gravity;
                Velocity += gravity * dt;
            }

            Position += Velocity * dt;
            TimeToLive -= dt;

            _owner.transform.position = Position;
            var angle = Mathf.Atan2(Velocity.y, Velocity.x) * Mathf.Rad2Deg;
            _owner.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

            if (TimeToLive <= 0) {
                _owner.ResetObject();
            }
        }

        public void CheckForTargets()
        {
            Collider2D[] targets = Physics2D.OverlapCircleAll(Position, FollowRange, 1 << LayerMask.NameToLayer("Entity"));

            float currentDistance = 0;
            GameObject chosenTarget = null;

            foreach (Collider2D potentialTarget in targets) {
                if(!_owner.Data.CollisionTags.Contains(potentialTarget.tag)) { continue; }

                float distance = Vector2.Distance(Position, (Vector2)potentialTarget.transform.position);

                if (chosenTarget == null || distance < currentDistance) {
                    currentDistance = distance;
                    chosenTarget = potentialTarget.gameObject;
                }
            }

            Target = chosenTarget;
        }

        public void Homeing(float dt)
        {
            if(Target == null) { return; }

            Speed += Acceleration * dt;
            Speed = Mathf.Clamp(Speed, -MaxSpeed, MaxSpeed);

            Vector2 desiredVelocity = (new Vector2(Target.transform.position.x, Target.transform.position.y) - Position).normalized;
            desiredVelocity *= Speed;

            Vector2 steer = desiredVelocity - Velocity;
            Velocity = Vector2.ClampMagnitude(Velocity + steer * FollowIntensity * dt, Speed);
        }
    }
}
