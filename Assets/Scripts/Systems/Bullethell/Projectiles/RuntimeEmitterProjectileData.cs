using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell;

namespace BulletHell.Emitters.Projectiles
{
    public class RuntimeEmitterProjectileData
    {
        EmitterProjectile _owner;

        public Vector2 Position;
        public Vector2 Velocity;
        public Vector2 Direction;
        public Vector2 GravityPoint;
        public float Gravity;
        public float TimeToLive;
        public float Acceleration;
        public float Speed;

        public void SetOwner(EmitterProjectile owner) => _owner = owner;

        public void UpdateData(float dt)
        {
            Vector2 gravity = (GravityPoint - Position).normalized * Gravity;
            Velocity += (Direction * Acceleration * dt) + gravity;
            Position += Velocity * dt;
            TimeToLive -= dt;

            _owner.transform.position = Position;
            var angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg;
            _owner.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

            if (TimeToLive <= 0) {
                _owner.ResetObject();
            }
        }
    }
}
