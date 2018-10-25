using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatcherAnimationController : MonoBehaviour {

    public float smoothTime = 0.1f;

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
        if (watcherRef.currentState.stateName == STATE.Idle)
        {
            watcherAnimator.SetFloat("animationPercentage", 0f);
        }
        if (watcherRef.currentState.stateName == STATE.Attack)
        {
            if (shootRef.TimeRateElapsed())
            {
                watcherAnimator.SetBool("attack",true);
            }
            else
            {
                watcherAnimator.SetBool("attack", false);
            } 
        }
	}
}
