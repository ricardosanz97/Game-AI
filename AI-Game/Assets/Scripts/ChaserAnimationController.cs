using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaserAnimationController : MonoBehaviour {

    public float smoothTime = 0.1f;

	private AudioSource audio;
	public AudioClip arañazo;
    private Animator chaserAnimator;
    private ChaserNPC chaserRef;

    void Start()
    {
		audio = GetComponent<AudioSource> ();


        chaserAnimator = GetComponent<Animator>();
        chaserRef = GetComponent<ChaserNPC>();
    }

    void Update()
    {
        if (chaserRef.currentState.stateName == STATE.Alert)
        {
            chaserAnimator.SetFloat("speedPercentage", 0f);
        }
        if (chaserRef.currentState.stateName == STATE.Patrol)
        {
            chaserAnimator.SetFloat("speedPercentage", 0.5f);

        }
        if(chaserRef.currentState.stateName == STATE.Attack)
        {
            chaserAnimator.SetFloat("speedPercentage", 1f);
			audio.clip = arañazo;
			audio.Play ();
        } 
    }
}
