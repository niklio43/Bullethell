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
        Emitter _target;
        Texture2D _emitterIcon;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            DrawDataInspector();
        }

        private void DrawDataInspector()
        {
            if (_target.Data != null) {
                var editor = CreateEditor(_target.Data);
                editor.OnInspectorGUI();
            }
        }

        void OnSceneGUI()
        {

        }

        private void OnEnable()
        {
            _target = (Emitter)target;
        }
    }
}
