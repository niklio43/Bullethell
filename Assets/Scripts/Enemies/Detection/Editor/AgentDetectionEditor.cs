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
            Vector3 viewAngleA = Utilities.DirFromAngle(-_target.Angle / 2, _target.transform.rotation.eulerAngles.z);
            Vector3 viewAngleB = Utilities.DirFromAngle(_target.Angle / 2, _target.transform.rotation.eulerAngles.z);

            Handles.color = Color.white;
            Handles.DrawWireArc(_target.transform.position, Vector3.forward, viewAngleA, _target.Angle, _target.Radius);

            Handles.color = Color.yellow;
            Handles.DrawWireArc(_target.transform.position, Vector3.forward, Vector3.up, 360, _target.innerRadius);

            Handles.color = Color.white;

            Handles.DrawLine(_target.transform.position, _target.transform.position + viewAngleA * _target.Radius);
            Handles.DrawLine(_target.transform.position, _target.transform.position + viewAngleB * _target.Radius);
        }

        private void OnEnable()
        {
            _target = (AgentDetection)target;
        }
    }
}
