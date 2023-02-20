using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using BulletHell.Emitters.Projectiles;

namespace BulletHell.Emitters
{
    [CreateAssetMenu(fileName = "EmitterData", menuName = "Emitters/EmitterData", order = 1)]
    public class EmitterData : ScriptableObject
    {
        //General
        public bool FoldOutGeneral = false;
        [Range(0, 5000)] public int Delay = 1000;
        [Range(0, 10)] public float RotationSpeed = 0;

        //Projectile
        public bool FoldOutProjectile = false;
        public ProjectileData ProjectileData;
        public float LifeTime = 5;

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
