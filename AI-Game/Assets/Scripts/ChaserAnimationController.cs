using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaserAnimationController : MonoBehaviour {

    public float smoothTime = 0.1f;

	private float currentTime = 0.0f;
	private AudioSource audio;
	//public AudioClip arañazo;
	public AudioClip gruñido;
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
			audio.clip = gruñido;

        }
        if(chaserRef.currentState.stateName == STATE.Attack)
        {
            chaserAnimator.SetFloat("speedPercentage", 1f);
			//audio.clip = arañazo;

        } 
		currentTime += Time.deltaTime;
		if (currentTime > 3.0f) {
			currentTime = 0.0f;
			audio.Play ();
		}

		
    }
}
