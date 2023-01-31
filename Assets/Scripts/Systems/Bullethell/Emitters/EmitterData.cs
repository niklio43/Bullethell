using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace BulletHell.Emitters
{
    [CreateAssetMenu(fileName = "EmitterData", menuName = "Emitters/EmitterData", order = 1)]
    public class EmitterData : ScriptableObject
    {
        //General
        public bool FoldOutGeneral = false;
        [Range(0, 5000)] public int Delay = 1000;
        [Range(0, 1000)] public int MaxProjectiles = 10;
        [Range(0, 10)] public float RotationSpeed = 0;

        //Projectile
        public bool FoldOutProjectile = false;
        public ProjectileData ProjectileData;
        public float TimeToLive = 5;
        [Range(0.01f, 100f)] public float Speed = 1;
        [Range(-100f, 100f)] public float Acceleration = 1;
        public float Gravity = 0;
        public Vector2 GravityPoint = Vector2.zero;

        //Emission
        public bool FoldOutEmitter = false;
        [Range(1, 40)] public int EmitterPoints = 1;
        [Range(-180, 180)] public float CenterRotation = 0;
        [Range(-180, 180)] public float Pitch = 0;
        [Range(0, 10)] public float Offset = 0;
        [Range(-180, 180)] public float Spread = 0;

        public float ParentRotation = 0;
        public Vector2 Direction = Vector2.up;
    }
}
