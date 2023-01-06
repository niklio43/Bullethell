using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BulletHell.Enemies.Detection
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
            if(_target.Stats != null)
            Draw();
        }

        private void Draw()
        {
            Handles.color = Color.red;
            Handles.DrawWireArc(_target.transform.position, Vector3.forward, Vector3.up, 360, _target.Stats.DetectionRadius);

            Handles.color = Color.yellow;
            Handles.DrawWireArc(_target.transform.position, Vector3.forward, Vector3.up, 360, _target.Stats.ObstacleDetectionRadius);
        }

        private void OnEnable()
        {
            _target = (Enemy)target;
        }
    }
}
