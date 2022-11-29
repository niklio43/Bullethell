using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.Emitters
{
    public class EmitterGroup
    {
        public Vector2 direction;
        public int spokeCount;
        public float spokeSpacing;
        
        public EmitterGroup(Vector2 direction, int spokeCount, float spokeSpacing)
        {
            this.direction = direction;
            this.spokeCount = spokeCount;
            this.spokeSpacing = spokeSpacing;
        }
    }
}