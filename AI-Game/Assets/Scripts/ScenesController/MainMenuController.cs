using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour {

	public void ButtonStartPressed()
    {
        SceneLoader.I.LoadScene(SceneLoader.SCENES.Level1);
    }

    public void ButtonOptionsPressed()
    {
        SceneLoader.I.LoadScene(SceneLoader.SCENES.Options);
    }

    public void ButtonCreditsPressed()
    {
        SceneLoader.I.LoadScene(SceneLoader.SCENES.Credits);
    }
}
