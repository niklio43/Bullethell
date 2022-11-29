using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.Emitters
{
    public class Emitter : BaseEmitter
    {
        [Header("Spokes")]
        [Range(1, 10)] protected int groupCount = 1;
        [Range(0, 1)] protected float groupSpacing = 1;
        [Range(1, 10)] protected int spokeCount = 3;
        [Range(0, 100)] protected float spokeSpacing = 25;

        EmitterGroup[] groups;

        private void Awake()
        {
            groups = new EmitterGroup[groupCount];

            for (int i = 0; i < groupCount; i++) {
                float rotation = 0;
                groups[i] = new EmitterGroup(Rotate(direction, rotation).normalized, spokeCount, spokeSpacing);

                rotation = CalculateGroupRotation(i, rotation);
            }

        }


        float CalculateGroupRotation(int index, float currentRotation)
        {
            currentRotation += 360 * groupSpacing / groupCount;
            return currentRotation;
        }


        protected override ProjectileData FireProjectile(Vector2 direction)
        {
            throw new System.NotImplementedException();
        }

        protected override void UpdateProjectile(ProjectileData projectile)
        {
            throw new System.NotImplementedException();
        }
    }
}
