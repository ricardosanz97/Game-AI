using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPlatformBehaviour : MonoBehaviour
{
    private LevelManager levelManager;

    private void Awake()
    {
        levelManager = GameObject.FindObjectOfType<LevelManager>().GetComponent<LevelManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            GameManager.I.levelCompleted = true;
        }     
    }

}
