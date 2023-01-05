using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using BulletHell;

namespace BulletHell.Enemies.Detection
{
    [CustomEditor(typeof(AgentDetection))]
    public class AgentDetectionEditor : Editor
    {
        AgentDetection _target;

        private void OnSceneGUI()
        {
            Draw();
        }

        private void Draw()
        {
            Handles.color = Color.red;
            Handles.DrawWireArc(_target.transform.position, Vector3.forward, Vector3.up, 360, _target.Radius);

            Handles.color = Color.yellow;
            Handles.DrawWireArc(_target.transform.position, Vector3.forward, Vector3.up, 360, _target.ObstacleDetectionRadius);
        }

        private void OnEnable()
        {
            _target = (AgentDetection)target;
        }
    }
}
