using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUDManager : Singleton<HUDManager> 
{
    public GameObject canvas;
    public Image YouDiedImage;
    public Image FadeImage;

    private void Awake()
    {
        EndHud();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneUnloaded(Scene scene)
    {
        if(scene.buildIndex == (int)SceneLoader.SCENES.Level1)
        {
            EndHud();
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.buildIndex == (int)SceneLoader.SCENES.Level1)
        {
            StartHud();
        }
    }
    
    private void EndHud()
    {
        canvas.SetActive(false);
    }
    
    private void StartHud()
    {
        canvas.SetActive(true);
    }
}
