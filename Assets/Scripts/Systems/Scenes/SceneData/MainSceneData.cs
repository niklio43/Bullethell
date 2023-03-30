using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.Scenes
{
    [CreateAssetMenu(fileName = "MainSceneData", menuName = "SceneData/MainSceneData", order = 0)]
    public class MainSceneData : SceneData
    {
        public List<SubSceneData> RequiredSubScenes = new List<SubSceneData>();
    }
}
