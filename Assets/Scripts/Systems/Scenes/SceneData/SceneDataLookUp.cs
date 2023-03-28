using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace BulletHell.Scenes
{
    public static class SceneDataLookUp
    {
        static Dictionary<int, SceneData> _lookUpChache;
        static void UpdateCache()
        {
            _lookUpChache = new Dictionary<int, SceneData>();
            SceneData[] sceneData = Resources.LoadAll<SceneData>("Scenes/SceneData");

            for (int i = 0; i < sceneData.Length; i++) {
                _lookUpChache.Add(sceneData[i].SceneReference.BuildIndex, sceneData[i]);
            }
        }

        public static SceneData LookUp(int buildIndex)
        {
            //First check if cache already contains sceneData
            if(_lookUpChache != null && _lookUpChache.ContainsKey(buildIndex))
                return _lookUpChache[buildIndex];

            //Update Cache
            UpdateCache();

            //Second check
            if (_lookUpChache.ContainsKey(buildIndex))
                return _lookUpChache[buildIndex];

            Debug.LogWarning("Could not find SceneData. Returning default...");
            return default(SceneData);
        }
    }
}
