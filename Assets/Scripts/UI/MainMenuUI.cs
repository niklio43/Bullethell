using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] Button _playButton;

    private void Start()
    {
        _playButton.onClick.AddListener(delegate () { LevelLoader.Instance.LoadLoading("Playtest"); });
    }

    public void ToggleObj(GameObject obj)
    {
        obj.SetActive(!obj.activeSelf);
    }
}
