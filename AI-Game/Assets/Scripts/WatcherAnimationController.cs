using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatcherAnimationController : MonoBehaviour {


    private Animator watcherAnimator;
    private WatcherNPC watcherRef;
    private ShootSteeringBehaviour shootRef;

	// Use this for initialization
	void Start () {
        watcherAnimator = GetComponent<Animator>();
        watcherRef = GetComponent<WatcherNPC>();
        shootRef = GetComponent<ShootSteeringBehaviour>();
    }
	
	// Update is called once per frame
	void Update () {
        
        if (watcherRef.currentState.stateName == STATE.Attack)
        {
            if (GetComponent<ShootSteeringBehaviour>().shotEnable)
            {
                watcherAnimator.SetTrigger("attackTrigger");
            }
        }
        else if (watcherRef.currentState.stateName == STATE.Idle)
        {
            watcherAnimator.SetFloat("animationPercentage", 0f);
        }
    }
}
