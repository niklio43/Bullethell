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
        [Range(0.01f, 100f)] public float Acceleration = 1;
        public float Gravity = 0;
        public Vector2 GravityPoint = Vector2.zero;

        //Emission
        public bool FoldOutEmitter = false;
        [Range(1, 40)] public int EmitterPoints = 1;
        [Range(-180, 180)] public float CenterRotation = 0;
        [Range(-180, 180)] public float Pitch = 0;
        [Range(0, 10)] public float Offset = 0;
        [Range(-180, 180)] public float Spread = 0;


        Projectile _projectilePrefab;
        public float ParentRotation = 0;
        public Vector2 Direction = Vector2.up;
        public Projectile ProjectilePrefab
        {
            get {
                if (_projectilePrefab == null) {
                    _projectilePrefab = Resources.Load<Projectile>("EmitterProjectile");
                }

                return _projectilePrefab;
            }
        }

        #region Modifier Handlers

        public List<EmitterModifier> Modifiers = new List<EmitterModifier>();

        public EmitterModifier AddModifier()
        {
            EmitterModifier newModifier;

#if UNITY_EDITOR
            newModifier = ScriptableObject.CreateInstance<EmitterModifier>();

            newModifier.guid = Guid.NewGuid().ToString();
            newModifier.name = $"Modifier { Modifiers.Count}";
            Modifiers.Add(newModifier);

            AssetDatabase.AddObjectToAsset(newModifier, this);
            AssetDatabase.SaveAssets();
#endif

            return newModifier;
        }

        public void DeleteModifier(int index)
        {
#if UNITY_EDITOR
            EmitterModifier modifierToDelete = Modifiers[index];

            AssetDatabase.RemoveObjectFromAsset(modifierToDelete);
            AssetDatabase.SaveAssets();
            Modifiers.Remove(modifierToDelete);
#endif
        }

        public void DeleteModifier(EmitterModifier modifier)
        {
#if UNITY_EDITOR
            AssetDatabase.RemoveObjectFromAsset(modifier);
            AssetDatabase.SaveAssets();
            Modifiers.Remove(modifier);
#endif
        }

        public void SwapIndices(int a, int b)
        {
            EmitterModifier holder = Modifiers[a];
            Modifiers[a] = Modifiers[b];
            Modifiers[b] = holder;
        }

        public void MoveUpModifier(EmitterModifier modifier)
        {
            int index = Modifiers.IndexOf(modifier);
            if (index == 0) { return; }

            SwapIndices(index, index - 1);
        }

        public void MoveDownModifier(EmitterModifier modifier)
        {
            int index = Modifiers.IndexOf(modifier);
            if (index == Modifiers.Count - 1) { return; }

            SwapIndices(index, index + 1);
        }

        public void ClearModifiers()
        {
            for (int i = 0; i < Modifiers.Count; i++) {
                DeleteModifier(i);
                i--;
            }
            Modifiers.Clear();
        }

        public EmitterModifier GetModifier(string name)
        {
            foreach (EmitterModifier modifier in Modifiers) {
                if(modifier.name == name) {
                    return modifier;
                }
            }

            Debug.LogError($"No modifier with the name: '{name}' could be found!");
            return null;
        }

        #endregion
    }
}
