using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[DisallowMultipleComponent]
public class GameManager : Singleton<GameManager>
{
    public bool playerAlive = true;
    public bool levelCompleted;
    public bool playerSpawned;
    public LevelManager currentLevel;

    private IEnumerator CheckPlayerAlive()
    {
        while (playerAlive)
        {
            yield return null;
        }
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

    public void RestartLevel(LevelManager startedLevel)
    {
        currentLevel = startedLevel;
        playerAlive = true;
        levelCompleted = false;
        startedLevel.KeyTaken = false;
        
        InitCoroutines();
        startedLevel.SpawnPlayer();
    }
}
