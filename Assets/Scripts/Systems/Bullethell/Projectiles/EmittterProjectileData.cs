using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Emitters;
using BulletHell.Stats;
using UnityEngine.VFX;

namespace BulletHell.Emitters.Projectiles
{
    [CreateAssetMenu(fileName = "ProjectileData", menuName = "Emitters/ProjectileData")]
    public class EmittterProjectileData : ScriptableObject
    {
        [Header("General")]
        public new string name = "NewProjectileData";
        public float Scale = 1;

        [Header("Visual")]
        public Sprite Sprite;
        public Color birth = Color.white;
        public Color midLife = Color.white;
        public Color Death = Color.white;

        [Header("Animation/VFX")]
        public AnimationClip AnimationClip;
        public VisualEffectAsset HitVFX;

        [Header("Collision")]
        public bool DestroyOnHit = true;
        public Bounds collider;
        public List<string> CollisionTags = new List<string>();
    }
}