using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.Emitters
{
    public class EmitterGroup
    {
        public Vector2 Position;
        public Vector2 Direction;

        public EmitterModifier Modifier;
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

        public void Set(Vector2 position, Vector2 direction)
        {
            this.Position = position;
            this.Direction = direction;
        }

        public void SetModifier(EmitterModifier modifier)
        {
            if(modifier == null) { return; }
            Modifier = modifier;
        }

        public void ClearModifier() => Modifier = null;
    }
}