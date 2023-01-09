using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace BulletHell.Emitters.Editor
{
    using Editor = UnityEditor.Editor;

    [CustomEditor(typeof(Emitter))]
    public class EmitterEditor : Editor
    {
        VisualElement _root { get; set; }
        VisualElement dataEditor;

        Emitter _target;
        Texture2D _emitterIcon;

        public override VisualElement CreateInspectorGUI()
        {
            CreateRoot();
            RepaintInspector();
            return _root;
        }

        private void CreateRoot()
        {
            _root = new VisualElement();

            PropertyField autoFireField = new PropertyField(serializedObject.FindProperty("AutoFire"));
            PropertyField dataField = new PropertyField(serializedObject.FindProperty("Data"));
            dataField.RegisterCallback<SerializedPropertyChangeEvent>((changeEvent) => RepaintInspector());
            _root.Add(autoFireField);
            _root.Add(dataField);
        }

        void DrawDataInspector()
        {
            if (_target.Data == null) { return; }
            var editor = CreateEditor(_target.Data);
            dataEditor = editor.CreateInspectorGUI();
            _root.Add(dataEditor);
        }

        void RepaintInspector()
        {
            if(dataEditor != null)
                dataEditor.Clear();
            
            DrawDataInspector();
        }

        private void OnEnable()
        {
            _target = (Emitter)target;
        }
    }
}
