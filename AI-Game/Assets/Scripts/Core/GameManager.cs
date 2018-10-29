using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class GameManager : Singleton<GameManager>
{
    public bool playerAlive = true;
    public bool levelCompleted;
    public bool playerSpawned;
    public LevelManager currentLevel;

    private void Start()
    {
        InitCoroutines();
    }

    private IEnumerator CheckPlayerAlive()
    {
        while (playerAlive)
        {
            yield return null;
        }
        yield return new WaitForSeconds(5f);
        currentLevel.PlayerDead();
    }

    private IEnumerator CheckLevelCompleted()
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

    public void StarLevel(LevelManager startedLevel)
    {
        currentLevel = startedLevel;
        playerAlive = true;
        levelCompleted = false;
        startedLevel.KeyTaken = false;
        
        InitCoroutines();
        startedLevel.SpawnPlayer();
    }
}
