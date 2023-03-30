using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;



namespace BulletHell.Scenes
{
    using SceneManager = UnityEngine.SceneManagement.SceneManager;
    public static class SceneLoader
    {
        public static MainSceneData MainScene = null;
        public static List<SubSceneData> LoadedSubScenes;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static async void Initialize()
        {
            LoadedSubScenes = new List<SubSceneData>();
            MainScene = null;

            Scene currentScene = SceneManager.GetActiveScene();
            SceneData data = SceneDataLookUp.LookUp(currentScene.buildIndex);

            if (data != null && data is MainSceneData) {
                MainSceneData mainSceneData = data as MainSceneData;
                MainScene = mainSceneData;

                foreach (SubSceneData subScene in mainSceneData.RequiredSubScenes) {
                    await LoadSubScene(subScene);
                }
            }
        }

        #region Public Methods
        public static async Task LoadMainScene(MainSceneData data)
        {
            //Unload current main scene.
            if (MainScene != null) {
                await UnloadScene(MainScene);
                MainScene = null;
            }

            //Load all Required Sub Scenes.
            foreach (SubSceneData subScene in data.RequiredSubScenes) {
                await LoadSubScene(subScene);
            }

            List<SubSceneData> subScenesToUnload = new List<SubSceneData>();

            for (int i = 0; i < LoadedSubScenes.Count; i++) {
                SubSceneData loadedSubScene = LoadedSubScenes[i];
                if (!data.RequiredSubScenes.Contains(loadedSubScene) && !loadedSubScene.Persistant) {
                    subScenesToUnload.Add(loadedSubScene);
                }
            }

            foreach (SubSceneData subScene in subScenesToUnload) {
                await UnloadScene(subScene);
            }

            //Setup is done, now we can load the main scene.
            await LoadScene(data.SceneReference.BuildIndex, true);
        }

        public static async Task LoadSubScene(SubSceneData data)
        {
            if (LoadedSubScenes.Contains(data)) return;
            await LoadScene(data.SceneReference.BuildIndex);

            LoadedSubScenes.Add(data);
        }
        #endregion

        #region Private Methods
        static async Task UnloadScene(SceneData data)
        {
            await SceneManager.UnloadSceneAsync(data.SceneReference.BuildIndex);

            if (data is SubSceneData) {
                SubSceneData subSceneData = data as SubSceneData;
                LoadedSubScenes.Remove(subSceneData);
            }
            else if (data is MainSceneData) {
                MainScene = null;
            }
        }

        static async Task LoadScene(int buildIndex, bool setActive = false)
        {
            await SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Additive);

            if (setActive == true) {
                Scene scene = SceneManager.GetSceneByBuildIndex(buildIndex);
                SceneManager.SetActiveScene(scene);
            }
        }

        #endregion
    }
}
