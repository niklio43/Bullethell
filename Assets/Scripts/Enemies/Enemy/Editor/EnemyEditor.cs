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
        }

        private void OnSceneGUI()
        {
            Draw();
        }

        private void Draw()
        {
            Handles.color = Color.red;
            Handles.DrawWireArc(_target.transform.position, Vector3.forward, Vector3.up, 360, _target.AttackDistance);

            Handles.color = Color.green;
            Handles.DrawWireArc(_target.transform.position, Vector3.forward, Vector3.up, 360, _target.PreferredDistance);
        }

        private void OnEnable()
        {
            _target = (Enemy)target;
        }
    }
}
