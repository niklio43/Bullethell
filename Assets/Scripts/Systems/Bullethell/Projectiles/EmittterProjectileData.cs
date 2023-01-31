using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Emitters;
using BulletHell.Stats;

namespace BulletHell.Emitters.Projectiles
{
    [CreateAssetMenu(fileName = "ProjectileData", menuName = "Emitters/ProjectileData")]
    public class EmittterProjectileData : ScriptableObject
    {
        public new string name = "NewProjectileData";

        public Sprite Sprite;
        public float Scale = 1;
        public Color Color = Color.white;
        public Bounds collider;

        public List<string> CollisionTags = new List<string>();
    }
}