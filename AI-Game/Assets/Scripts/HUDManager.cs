using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class HUDManager : Singleton<HUDManager> 
{
    public GameObject canvas;
    public Image YouDiedImage;
    public Text YouDiedText;
    public Text LevelCompletedText;
    public Image FadeImage;
    public Image FadeImageLevelCompleted;
    public Slider PlayerHealthSlider;
    public Image FillImage;
    public Image RedFlash;
    public Image LoadingBar;

    private void Awake()
    {
        ResetHUD();
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

    public void ResetHUD()
    {
        YouDiedImage.DOFade(0f, 0f);
        YouDiedText.DOFade(0f, 0f);
        LevelCompletedText.DOFade(0f, 0f);
        FadeImage.DOFade(1f, 0f);
        FadeImageLevelCompleted.DOFade(0f, 0f);
    }
}
