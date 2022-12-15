using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BulletHell.Emitters
{
    public class EmitterModifier : ScriptableObject
    {
        public bool FoldOut = false;
        public string guid { get; set; }
        public bool Enabled = true;

        //General
        public bool FoldOutGeneral = false;
        public int Factor;
        public int Count;

        //Projectile Modifiers
        public bool FoldOutProjectile = false;
        public ProjectileData ProjectileData;
        [Range(0.01f, 100f)] public float Speed = 1;
        public float Acceleration;
        public float Gravity;
        public Vector2 GravityPoint;

        //Emission Modifiers
        public bool FoldOutEmitter = false;
        [Range(-180, 180)] public float Pitch = 0;
        [Range(0, 10)] public float Offset = 0;
        [Range(-180, 180)] public float NarrowSpread = 0;


        public void Rename(string newName)
        {
            if(newName == name) { return; }
            name = newName;
#if UNITY_EDITOR
            AssetDatabase.SaveAssets();
#endif
        }
    }
}