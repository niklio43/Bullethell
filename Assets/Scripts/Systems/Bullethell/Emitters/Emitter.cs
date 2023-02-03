using System.Collections.Generic;
using UnityEngine;
using BulletHell.Stats;
using BulletHell.Emitters.Projectiles;
using BulletHell.StatusSystem;

namespace BulletHell.Emitters
{
    public class Emitter
    {
        GameObject _owner;
        Transform _transform;
        EmitterData _data;
        List<EmitterProjectile> _activeProjectiles;
        EmitterGroupsManager _emitterGroups;

        public Emitter(GameObject owner, EmitterData data)
        {
            _owner = owner;
            _data = data;

            _transform = _owner.transform;
            _activeProjectiles = new List<EmitterProjectile>();
            _emitterGroups = new EmitterGroupsManager(owner.transform, _data);
        }

        public void Uninitialize() => ClearAllProjectiles();

        public void UpdateEmitter(float dt)
        {
            _emitterGroups.UpdateGroups();
        }

        public void SetDirection(Vector2 direction)
        {
            _data.Direction = direction;
            _emitterGroups.UpdateGroups();
        }

        public virtual void FireProjectile(Character projectileOwner = null, DamageInfo damage = null, List<StatusEffect> statusEffects = null)
        {
            for (int i = 0; i < Mathf.Clamp(_data.EmitterPoints, 0, _data.MaxProjectiles); i++) {
                EmitterProjectile projectile = ProjectileManager.Instance.Get();
                _activeProjectiles.Add(projectile);

                RuntimeEmitterProjectileData runTimeData = new RuntimeEmitterProjectileData();

                runTimeData.Position = _emitterGroups[i].Position;
                runTimeData.Direction = _emitterGroups[i].Direction;
                runTimeData.Speed = _data.Speed;
                runTimeData.Acceleration = _data.Acceleration;
                runTimeData.Velocity = runTimeData.Direction * runTimeData.Speed;
                runTimeData.Gravity = _data.Gravity;
                runTimeData.GravityPoint = _data.GravityPoint;
                runTimeData.TimeToLive = _data.TimeToLive;
                runTimeData.FollowTarget = _data.FollowTarget;
                runTimeData.FollowRange = _data.FollowRange;
                runTimeData.FollowIntensity = _data.FollowIntensity;
                runTimeData.MaxSpeed = _data.MaxSpeed;

                projectile.Initialize(_data.ProjectileData, runTimeData);
                projectile.SetOwner(projectileOwner);
                projectile.SetDamage(damage);
                projectile.SetStatusEffect(statusEffects);
            }
        }

        protected void ReturnProjectile(EmitterProjectile projectile) => projectile.ResetObject();
        public void ClearAllProjectiles()
        {
            foreach (EmitterProjectile projectile in _activeProjectiles) {
                projectile.ResetObject();
            }
        }
    }
}
