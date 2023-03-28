using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BulletHell.Scenes
{
    public abstract class SceneData : ScriptableObject
    {
        public SceneReference SceneReference;
    }
}