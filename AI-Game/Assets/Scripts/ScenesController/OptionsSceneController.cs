using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsSceneController : MonoBehaviour {

	public void BackButtonPressed()
    {
        SceneLoader.I.LoadScene(SceneLoader.SCENES.Menu);
    }
}
