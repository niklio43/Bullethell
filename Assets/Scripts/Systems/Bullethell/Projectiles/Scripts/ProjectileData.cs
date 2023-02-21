using System.Collections.Generic;
using UnityEngine;
using BulletHell.Emitters.Projectiles.Behaviours;
using UnityEditor;

namespace BulletHell.Emitters.Projectiles
{
    [CreateAssetMenu(fileName = "ProjectileData", menuName = "Emitters/New Projectile Data")]
    public class ProjectileData : ScriptableObject
    {
        //General
        public bool FoldOutGeneral = false;
        public new string name = "NewProjectileData";
        public float Scale = 1;

        //Sprite
        public bool FoldOutSprite = false;
        public Sprite Sprite;
        public Color Birth = Color.white;
        public Color MidLife = Color.white;
        public Color Death = Color.white;

        //Behaviour
        public bool FoldOutBehaviour = false;
        public float Speed;
        public float MaxSpeed;
        public float Acceleration;
        public List<BaseProjectileBehaviour> Behaviours = new List<BaseProjectileBehaviour>();

        //Collision
        public bool FoldOutCollision = false;
        public List<string> CollisionTags = new List<string>();
        public Bounds Collider;

        public void AddBehaviour(BaseProjectileBehaviour behaviour)
        {
            if (AlreadyHasBehaviour(behaviour)) return;
#if UNITY_EDITOR
            Behaviours.Add(behaviour);
            behaviour.SetOwner(this);
            AssetDatabase.AddObjectToAsset(behaviour, this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
#endif
        }

        public void RemoveBehaviour(BaseProjectileBehaviour behaviour)
        {
            if (!Behaviours.Contains(behaviour)) return;
#if UNITY_EDITOR
            Behaviours.Remove(behaviour);
            AssetDatabase.RemoveObjectFromAsset(behaviour);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
#endif
        }



        public bool AlreadyHasBehaviour(BaseProjectileBehaviour behaviour)
        {
            foreach (BaseProjectileBehaviour existingBehaviour in Behaviours) {
                if (existingBehaviour.Id() == behaviour.Id()) return true;
            }

            return false;
        }


    }
}
