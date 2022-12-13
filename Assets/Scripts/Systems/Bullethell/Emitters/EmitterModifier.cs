using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BulletHell.Emitters
{
    [CreateAssetMenu(fileName = "ModifierData", menuName = "Emitters/ModifierData", order = 1)]
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
        public float TimeToLive = 0;
        [Range(0.1f, 5f)] public float SpeedMultiplier = 1;

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