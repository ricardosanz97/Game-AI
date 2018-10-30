using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommanderAnimationController : MonoBehaviour {

    public float smoothTime = 0.1f;

    private Animator commanderAnimator;
    private FacePlayerSteeringBehaviour facePlayerRef;

    // Use this for initialization
    void Start()
    {
        commanderAnimator = GetComponent<Animator>();
        facePlayerRef = GetComponent<FacePlayerSteeringBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<DetectPlayerCondition>().playerDetected)
        {
            commanderAnimator.SetBool("detectPlayer", true);
            Debug.Log(GetComponent<DetectPlayerCondition>().playerDetected);
        }
        else
        {
            commanderAnimator.SetBool("detectPlayer", false);
            Debug.Log(GetComponent<DetectPlayerCondition>().playerDetected);
        }
    }
}
