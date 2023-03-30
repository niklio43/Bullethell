using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BulletHell.Enemies
{
    [CustomEditor(typeof(Enemy))]
    public class EnemyEditor : Editor
    {
        #region Private Fields
        Enemy _target;
        #endregion

        #region Public Methods
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
        }
        #endregion

        #region Private Methods
        private void OnSceneGUI()
        {
            Draw();
        }

        private void Draw()
        {
            Handles.color = Color.green;
            Handles.DrawWireArc(_target.transform.position, Vector3.forward, Vector3.up, 360, _target.PreferredDistance);
        }

        private void OnEnable()
        {
            _target = (Enemy)target;
        }
        #endregion
    }
}
