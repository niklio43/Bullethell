using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletHell.Emitters;

namespace BulletHell
{
    [CreateAssetMenu(fileName = "ProjectileData", menuName = "Emitters/ProjectileData")]
    public class ProjectileData : ScriptableObject
    {
        public new string name = "NewProjectileData";

        public Sprite Sprite;
        public RuntimeAnimatorController Animator;
        public float Scale = 1;
        public Color Color = Color.white;
    }
}