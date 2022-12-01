using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Emitters;

namespace BulletHell
{
    [Serializable]
    public class ProjectileData
    {
        public Vector2 velocity;
        public float acceleration;
        public Vector2 gravity;
        public Vector2 position;
        public float timeToLive;
        public float speed;
    }
}