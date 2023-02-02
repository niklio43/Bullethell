using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace BulletHell.Emitters.Editor
{
    using Editor = UnityEditor.Editor;

    [CustomEditor(typeof(EmitterData))]
    public class EmitterDataEditor : Editor
    {
        VisualElement _root;
        EmitterData _target;

        public override VisualElement CreateInspectorGUI()
        {
            if(_target != null)
                Redraw();

            return _root;
        }

        private void Redraw()
        {
            CreateRoot();
            CreateDefualtInspector();
        }

        private void CreateRoot()
        {
            _root = new VisualElement();
            VisualTreeAsset original = Resources.Load<VisualTreeAsset>("EmitterInspector");
            StyleSheet sheet = Resources.Load<StyleSheet>("EmitterDataStyleSheet");
            _root.styleSheets.Add(sheet);
            TemplateContainer treeAsset = original.CloneTree();
            _root.Add(treeAsset);
        }

        void CreateDefualtInspector()
        {
            Label emitterName = _root.Query<Label>("emitter-name").First();
            emitterName.text = _target.name;

            GroupBox dataRoot = _root.Query<GroupBox>("emitter-data").First();

            #region General Data Foldout
            Foldout generalFoldout = new Foldout();
            generalFoldout.value = _target.FoldOutGeneral;
            generalFoldout.name = "DefaultData_Foldout";
            generalFoldout.text = "General";
            generalFoldout.RegisterCallback<ClickEvent>((changeEvent) => _target.FoldOutGeneral = generalFoldout.value);

            generalFoldout.Add(EditorExtensions.CreatePropertyField(serializedObject.FindProperty("Delay")));
            generalFoldout.Add(EditorExtensions.CreatePropertyField(serializedObject.FindProperty("MaxProjectiles")));
            generalFoldout.Add(EditorExtensions.CreatePropertyField(serializedObject.FindProperty("RotationSpeed")));
            dataRoot.Add(generalFoldout);
            #endregion

            #region Projectile Data Foldout
            Foldout projectileFoldout = new Foldout();
            projectileFoldout.value = _target.FoldOutProjectile;
            projectileFoldout.name = "DefaultData_Foldout";
            projectileFoldout.text = "Projectile";
            projectileFoldout.RegisterCallback<ClickEvent>((changeEvent) => _target.FoldOutProjectile = projectileFoldout.value);

            projectileFoldout.Add(EditorExtensions.CreatePropertyField(serializedObject.FindProperty("ProjectileData")));
            projectileFoldout.Add(EditorExtensions.CreatePropertyField(serializedObject.FindProperty("TimeToLive")));
            projectileFoldout.Add(EditorExtensions.CreatePropertyField(serializedObject.FindProperty("Speed")));
            projectileFoldout.Add(EditorExtensions.CreatePropertyField(serializedObject.FindProperty("Acceleration")));
            projectileFoldout.Add(EditorExtensions.CreatePropertyField(serializedObject.FindProperty("Gravity")));
            projectileFoldout.Add(EditorExtensions.CreatePropertyField(serializedObject.FindProperty("GravityPoint")));
            dataRoot.Add(projectileFoldout);
            #endregion

            #region Emission Data Foldout
            Foldout emissionFoldout = new Foldout();
            emissionFoldout.value = _target.FoldOutEmitter;
            emissionFoldout.name = "DefaultData_Foldout";
            emissionFoldout.text = "Emission";
            emissionFoldout.RegisterCallback<ClickEvent>((changeEvent) => _target.FoldOutEmitter = emissionFoldout.value);

            emissionFoldout.Add(EditorExtensions.CreatePropertyField(serializedObject.FindProperty("EmitterPoints")));
            emissionFoldout.Add(EditorExtensions.CreatePropertyField(serializedObject.FindProperty("CenterRotation")));
            emissionFoldout.Add(EditorExtensions.CreatePropertyField(serializedObject.FindProperty("Pitch")));
            emissionFoldout.Add(EditorExtensions.CreatePropertyField(serializedObject.FindProperty("Offset")));
            emissionFoldout.Add(EditorExtensions.CreatePropertyField(serializedObject.FindProperty("Spread")));
            dataRoot.Add(emissionFoldout);
            #endregion

        }

        private void OnEnable()
        {
            _target = target as EmitterData;
        }
    }
}
