using UnityEditor;
using UnityEngine;

namespace BulletHell.Emitters.Editor
{
    using Editor = UnityEditor.Editor;

    [CustomEditor(typeof(Emitter))]
    public class EmitterEditor : Editor
    {
        Emitter _target;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            DrawDataInspector();
        }

        void DrawDataInspector()
        {
            if(_target.data != null) {
                var editor = CreateEditor(_target.data);
                editor.OnInspectorGUI();
            }
        }

        private void OnEnable()
        {
            _target = (Emitter)target;
        }
    }
}
