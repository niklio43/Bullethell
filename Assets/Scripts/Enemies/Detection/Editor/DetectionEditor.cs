using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BulletHell.Enemies.Detection
{
    [CustomEditor(typeof(EnemyDetection))]
    public class DetectionEditor : Editor
    {
        EnemyDetection _target;

        private void OnSceneGUI()
        {
            Draw();
        }

        private void Draw()
        {
            if(!_target.DrawGizmos) { return; }
            Handles.color = Color.white;
            Handles.DrawWireArc(_target.transform.position, Vector3.forward, Vector3.up, 360, _target.DetectionRadius);

            Handles.color = Color.yellow;
            Handles.DrawWireArc(_target.transform.position, Vector3.forward, Vector3.up, 360, _target.ObstacleDetectionRadius);
        }

        private void OnEnable()
        {
            _target = (EnemyDetection)target;
        }
    }
}
