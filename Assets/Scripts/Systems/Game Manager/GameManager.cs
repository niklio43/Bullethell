using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    #region Public Fields
    public static GameStates gameState = GameStates.Playing;
    public delegate void ChangeHandler(GameStates state);
    public static event ChangeHandler onStateChange;
    [HideInInspector] public bool pause;
    #endregion

    #region Private Fields
    [SerializeField] InputAction pauseGame;
    #endregion

    #region Private Methods
    void Awake()
    {
        //SetCursor();
        pauseGame.Enable();

        pauseGame.performed += ctx => PauseGame();
    }

    //void SetCursor()
    //{
    //    Texture2D tex = Resources.Load<Texture2D>("crosshair");
    //    Vector2 hotspot = new Vector2(tex.width / 2, tex.height / 2);
    //    Cursor.SetCursor(tex, hotspot, CursorMode.ForceSoftware);
    //}


    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        var buildIndex = SceneManager.GetActiveScene().buildIndex;
        //add scene logic
    }

    void Update()
    {
        if (gameState == GameStates.Playing) { pause = true; } else { pause = false; }
    }
    #endregion

    #region Public Methods
    public void PauseGame()
    {
        if (gameState == GameStates.Menu || gameState == GameStates.Loading) { return; }
        //if (Camera.main.GetComponent<CameraController>().IsCinematic) { return; }
        if (Instance.pause)
        {
            //AudioManager.instance.PlayOnce("Pause");
            SetTime(false);
            gameState = GameStates.Paused;
        }
        else
        {
            //AudioManager.instance.PlayOnce("Unpause");
            SetTime(true);
            gameState = GameStates.Playing;
            onStateChange?.Invoke(gameState);
        }
        onStateChange?.Invoke(gameState);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
#else
        
         Application.Quit();
#endif

    }

    public void SetTime(bool scale)
    {
        if (!scale)
            Time.timeScale = 0f;

        if (scale)
            Time.timeScale = 1f;
    }
    #endregion
}

public enum GameStates
{
    Paused,
    Playing,
    Loading,
    Menu
}