using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

[RequireComponent(typeof(GameManager),typeof(SceneLoader))]
[DisallowMultipleComponent]
public class Loader : Singleton<Loader> {
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        SceneLoader.I.LoadScene(SceneLoader.SCENES.Menu);
    }

}
