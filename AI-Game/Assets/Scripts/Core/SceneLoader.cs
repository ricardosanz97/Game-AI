using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class SceneLoader : Singleton<SceneLoader>
{
    public enum SCENES
    {
        Loader = 0,
        Menu = 1,
        Level1 = 2
    }

    public SCENES CurrentScene { get; private set; }

    private void Awake()
    {
        CurrentScene = SCENES.Loader;
    }

    public void LoadScene(SCENES sceneToLoad)
    {
        CurrentScene = sceneToLoad;
        SceneManager.LoadScene((int)sceneToLoad);
    }
}
