using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    public static GameStates gameState = GameStates.Playing;

    [SerializeField] InputAction pauseGame;

    public delegate void ChangeHandler(GameStates state);
    public static event ChangeHandler onStateChange;

    [HideInInspector] public bool pause;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        pauseGame.Enable();

        pauseGame.performed += ctx => PauseGame();
    }

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

    //temporary function, replace with async system later
    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
}

public enum GameStates
{
    Paused,
    Playing,
    Loading,
    Menu
}