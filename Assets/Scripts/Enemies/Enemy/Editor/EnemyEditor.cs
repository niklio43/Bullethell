using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BulletHell.Enemies
{
    [CustomEditor(typeof(Enemy))]
    public class EnemyEditor : Editor
    {
        Enemy _target;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if(_target.Stats != null) {
                var editor = Editor.CreateEditor(_target.Stats);
                editor.OnInspectorGUI();
            }

        }

        private void OnSceneGUI()
        {
            if(_target.Stats != null)
            Draw();
        }

        private void Draw()
        {
            Handles.color = Color.red;
            Handles.DrawWireArc(_target.transform.position, Vector3.forward, Vector3.up, 360, _target.Stats.AttackDistance);

            Handles.color = Color.green;
            Handles.DrawWireArc(_target.transform.position, Vector3.forward, Vector3.up, 360, _target.Stats.PreferredDistance);
        }

        private void OnEnable()
        {
            _target = (Enemy)target;
        }
    }
}
