using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssassinAnimationController : MonoBehaviour {

    private AssassinNPC _assassinRef;
    private Animator _assassinAnimator;
	// Use this for initialization
	void Start () {

        _assassinRef = GetComponent<AssassinNPC>();
        _assassinAnimator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        if (_assassinRef.currentState.stateName == STATE.Attack)
        {
            if (GetComponent<HitSteeringBehaviour>().hitEnabled)
            {
                _assassinAnimator.SetTrigger("biteEnabled");
            }
        }
    }
}
