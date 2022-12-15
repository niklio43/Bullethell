using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.Emitters
{
    public class EmitterGroup
    {
        public Vector2 Position;
        public Vector2 Direction;

        public EmitterModifier Modifier { get; private set; }
        public bool HasModifier => (Modifier != null);
        
        public EmitterGroup(Vector2 position, Vector2 direction)
        {
            Set(position, direction);
        }

        public EmitterGroup()
        {
            Position = Vector2.zero;
            Direction = Vector2.zero;
        }

        public void Set(Vector2 position, Vector2 direction, EmitterModifier modifier = null)
        {
            this.Position = position;
            this.Direction = direction;
            Modifier = modifier;
        }

        public void ClearModifier() => Modifier = null;
    }
}