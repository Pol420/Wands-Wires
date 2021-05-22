using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    private static LevelManager instance;
    public static LevelManager Instance() { return instance; }
    public static UnityEvent levelLoad;
    public static UnityEvent levelReset;
    public static bool paused;

    private GameObject loadingScreen;
    private GameObject pauseScreen;
    private Slider loadBar;
    private AsyncOperation loading;
    private bool reset;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            loadingScreen = transform.GetChild(0).gameObject;
            pauseScreen = transform.GetChild(1).gameObject;
            loadBar = loadingScreen.GetComponentInChildren<Slider>();
            loadingScreen.SetActive(false);
            levelLoad = new UnityEvent();
            levelReset = new UnityEvent();
            reset = false;
            pauseScreen.SetActive(false);
            paused = false;
            levelLoad.AddListener(OnLevelLoad);
        }
        else if (instance != this) Destroy(gameObject);
    }

    private void Update()
    {
        if (loading != null)
        {
            if (loading.progress >= 1f)
            {
                loading = null;
                loadingScreen.SetActive(false);
                if (reset) levelReset.Invoke();
                else levelLoad.Invoke();
                reset = false;
            }
            else loadBar.value = Mathf.Clamp01(loading.progress / 0.9f);
        }
        else if (Input.GetButtonDown("Pause")) Pause();
    }

    public void Pause()
    {
        if(!InMainMenu())
        {
            if(!paused)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                pauseScreen.SetActive(true);
                paused = true;
                Time.timeScale = 0f;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                pauseScreen.SetActive(false);
                paused = false;
                Time.timeScale = 1f;
            }
        }
    }

    public void GoToMainMenu() { LoadScene(0); }
    public void LoadNextScene() { LoadScene((CurrentScene() + 1) % (SceneManager.sceneCount + 1)); }
    public void ReloadScene() { reset = true; LoadScene(CurrentScene()); }
    public void LoadScene(string sceneName) { LoadScene(SceneManager.GetSceneByName(sceneName).buildIndex); }
    public bool InMainMenu() { return CurrentScene() == 0; }
    private int CurrentScene() { return SceneManager.GetActiveScene().buildIndex; }
    public void LoadScene(int index)
    {
        loading = SceneManager.LoadSceneAsync(index, LoadSceneMode.Single);
        loadingScreen.SetActive(true);
        if (paused) Pause();
    }
    public void Quit() { Application.Quit(); }

    private void OnLevelLoad()
    {
        if (InMainMenu() && PlayerStats.Instance() != null)
        {
            Destroy(PlayerStats.Instance().gameObject);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
