using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : Singleton<LevelLoader>
{
    #region Public Methods
    public async void LoadLoading(string sceneName)
    {
        var scene = SceneManager.LoadSceneAsync("Loading");
        scene.allowSceneActivation = false;

        do
        {
            await Task.Delay(100);

        } while (scene.progress < 0.9f);

        scene.allowSceneActivation = true;
        LoadScene(sceneName);

    }

    public async void LoadScene(string sceneName)
    {
        var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;

        do
        {
            await Task.Delay(100);
            GameObject.FindGameObjectWithTag("Loading").GetComponent<Slider>().value = scene.progress;

        } while (scene.progress < 0.9f);

        scene.allowSceneActivation = true;
    }
    #endregion
}