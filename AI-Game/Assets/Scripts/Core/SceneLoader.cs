using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader>
{
    public enum SCENES
    {
        Menu = 0,
        Options = 1,
        Credits = 2,
        Level1 = 3
    }

    public SCENES currentScene = SCENES.Menu;
    
    public void LoadScene(SCENES sceneToLoad)
    {
        SceneManager.LoadScene((int)sceneToLoad);
    }

}
