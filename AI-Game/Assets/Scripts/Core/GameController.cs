using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    public bool playerAlive = true;
    public bool levelCompleted;
    public bool playerSpawned;
    public LevelManager currentLevel;

    private void Start()
    {
        InitCoroutines();
    }

    IEnumerator CheckPlayerAlive()
    {
        while (playerAlive)
        {
            yield return null;
        }
        yield return new WaitForSeconds(5f);
        currentLevel.PlayerDead();
    }

    IEnumerator CheckLevelCompleted()
    {
        while (!levelCompleted)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        currentLevel.LevelCompleted();
    }

    public void InitCoroutines()
    {
        StartCoroutine(CheckPlayerAlive());
        StartCoroutine(CheckLevelCompleted());
    }
}
