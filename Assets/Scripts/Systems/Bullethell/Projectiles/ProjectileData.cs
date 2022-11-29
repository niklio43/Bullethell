using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Emitters;

namespace BulletHell
{
    public class ProjectileData
    {
        public Vector2 velocity;
        public float acceleration;
        public Vector2 gravity;
        public Vector2 position;
        public float rotation;
        public float TimeToLive;
        public float speed;
        public float angularVelocity;

        public Transform target;
        public bool followTarget;
        public float followIntensity;
    }
}