using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell.Scenes
{
    [CreateAssetMenu(fileName = "SubSceneData", menuName = "SceneData/SubSceneData", order = 0)]
    public class SubSceneData : SceneData
    {
        public bool Persistant = false;
    }
}
