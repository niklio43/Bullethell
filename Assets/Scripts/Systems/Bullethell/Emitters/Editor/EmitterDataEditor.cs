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
            CreateDefaltInspector();
            CreateModifierInspectors();
        }

        private void CreateRoot()
        {
            _root = new VisualElement();
            VisualTreeAsset original = Resources.Load<VisualTreeAsset>("EmitterInspector");
            StyleSheet sheet = Resources.Load<StyleSheet>("EmitterDataStyleSheet");
            _root.styleSheets.Add(sheet);
            TemplateContainer treeAsset = original.CloneTree();
            _root.Add(treeAsset);

            Button addModifierBtn = _root.Query<Button>("button_add_modifier");
            addModifierBtn.clicked += () => { AddNewModifier(); };

            Button clearModifierBtn = _root.Query<Button>("button_clear_modifier");
            clearModifierBtn.clicked += () => { ClearModifiers(); };
        }

        void CreateDefaltInspector()
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

        void CreateModifierInspectors()
        {
            List<EmitterModifier> modifiers = _target.Modifiers;
            ScrollView modifierList = _root.Query<ScrollView>("modifiers_list").First();
            modifierList.Clear();

            for (int i = 0; i < modifiers.Count; i++) {
                modifierList.Add(CreateModifierFoldout(modifiers[i]));
            }
        }

        VisualElement CreateModifierFoldout(EmitterModifier modifier)
        {
            Editor editor = CreateEditor(modifier);

            VisualElement modifierRoot = editor.CreateInspectorGUI();

            GroupBox header = modifierRoot.Query<GroupBox>("modifier-header");
            Button moveUpBtn = modifierRoot.Query<Button>("move-up");
            Button moveDownBtn = modifierRoot.Query<Button>("move-down");
            Button deleteBtn = modifierRoot.Query<Button>("delete-modifier");

            moveUpBtn.clicked += () => { MoveUpModifier(modifier); };
            moveDownBtn.clicked += () => { MoveDownModifier(modifier); };
            deleteBtn.clicked += () => { RemoveModifier(modifier); };

            return modifierRoot;
        }

        void AddNewModifier()
        {
            _target.AddModifier();
            EditorUtility.SetDirty(_target);
            CreateModifierInspectors();
        }

        void RemoveModifier(EmitterModifier modifier)
        {
            _target.DeleteModifier(modifier);
            EditorUtility.SetDirty(_target);
            CreateModifierInspectors();
        }

        void MoveUpModifier(EmitterModifier modifier)
        {
            _target.MoveUpModifier(modifier);
            EditorUtility.SetDirty(_target);
            CreateModifierInspectors();
        }

        void MoveDownModifier(EmitterModifier modifier)
        {
            _target.MoveDownModifier(modifier);
            EditorUtility.SetDirty(_target);
            CreateModifierInspectors();
        }

        void ClearModifiers()
        {
            _target.ClearModifiers();
            EditorUtility.SetDirty(_target);
            CreateModifierInspectors();
        }

        private void OnEnable()
        {
            _target = target as EmitterData;
        }
    }
}
