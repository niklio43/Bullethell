using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.Emitters
{
    public class EmitterGroup
    {
        public Vector2 position;
        public Vector2 direction;
        
        public EmitterGroup(Vector2 position, Vector2 direction)
        {
            Set(position, direction);
        }

        public void Set(Vector2 position, Vector2 direction)
        {
            this.position = position;
            this.direction = direction;
        }
    }
}