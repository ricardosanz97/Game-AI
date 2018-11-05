using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIBehaviour : MonoBehaviour 
{
    public GameObject optionsPanel;
    public GameObject mainPanel;
    public GameObject UIRootGameObject;
	private AudioSource audio;
	public AudioClip menumusic;
    
    private void Awake()
    {
		audio = GetComponent<AudioSource> ();
        EventSystem eventSystem = FindObjectOfType<EventSystem>();


        if(eventSystem == null)
        {

            Debug.LogError("Not event system Found, creating One...");

            GameObject go = new GameObject("Event System");
            go.AddComponent<EventSystem>();
            go.AddComponent<StandaloneInputModule>();
            
        }
    }

    
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
		audio.clip = menumusic;
		audio.Play ();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

    }

    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.buildIndex != (int)SceneLoader.SCENES.Menu)
        {
            TurnOffUI();

            return;
        }
        
        TurnOnUI();
    }


    public void ButtonStartPressed()
    {
		audio.Stop ();
        SceneLoader.I.LoadScene(SceneLoader.SCENES.Level1);
    }
    
    public void ButtonCreditsPressed()
    {
        //TODO activar y desactivar paneles
    }

    public void ButtonOptionsPressed()
    {
        DeactivatePanel(mainPanel);
        ActivatePanel(optionsPanel);
    }

    public void BackButtonPressed()
    {
        DeactivatePanel(optionsPanel);
        ActivatePanel(mainPanel);
    }

    private void DeactivatePanel(GameObject panel)
    {
        panel.SetActive(false);
    }
    
    private void ActivatePanel(GameObject panel)
    {
        panel.SetActive(true);
    }
    
    private void TurnOnUI()
    {
        UIRootGameObject.SetActive(true);
    }
    private void TurnOffUI()
    {
        UIRootGameObject.SetActive(false);
    }
}
