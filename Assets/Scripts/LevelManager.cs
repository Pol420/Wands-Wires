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

    private GameObject loadingScreen;
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
            loadBar = loadingScreen.GetComponentInChildren<Slider>();
            loadingScreen.SetActive(false);
            levelLoad = new UnityEvent();
            levelReset = new UnityEvent();
            reset = false;
        }
        else if (instance != this) Destroy(gameObject);
    }

    private void Update()
    {
        if(loading != null)
        {
            if(loading.progress >= 1f)
            {
                loading = null;
                loadingScreen.SetActive(false);
                if (reset) levelReset.Invoke();
                else levelLoad.Invoke();
                reset = false;
            }
            else loadBar.value = Mathf.Clamp01(loading.progress / 0.9f);
        }
    }

    public void LoadNextScene() { LoadScene(CurrentScene() + 1); }
    public void ReloadScene() { reset = true; LoadScene(CurrentScene()); }
    public void LoadScene(string sceneName) { LoadScene(SceneManager.GetSceneByName(sceneName).buildIndex); }

    private int CurrentScene() { return SceneManager.GetActiveScene().buildIndex; }
    public void LoadScene(int index)
    {
        loading = SceneManager.LoadSceneAsync(index, LoadSceneMode.Single);
        loadingScreen.SetActive(true);
    }
}
