using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.Emitters
{
    public class EmitterGroup
    {
        #region Public Fields
        public Vector2 Position;
        public Vector2 Direction;

        public EmitterGroup(Vector2 position, Vector2 direction)
        {
            Set(position, direction);
        }

        public EmitterGroup()
        {
            Position = Vector2.zero;
            Direction = Vector2.zero;
        }
        #endregion

        #region Public Methods
        public void Set(Vector2 position, Vector2 direction)
        {
            this.Position = position;
            this.Direction = direction;
        }
        #endregion
    }
}