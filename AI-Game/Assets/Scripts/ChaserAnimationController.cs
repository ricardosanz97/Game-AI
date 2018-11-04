using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaserAnimationController : MonoBehaviour {

    public float SmoothTime = 0.1f;

	private float _currentTime = 0.0f;
	private AudioSource _audio;
	//public AudioClip arañazo;
	public AudioClip Gruñido;
    private Animator _chaserAnimator;
    private ChaserNPC _chaserRef;

    void Start()
    {
		_audio = GetComponent<AudioSource> ();


        _chaserAnimator = GetComponent<Animator>();
        _chaserRef = GetComponent<ChaserNPC>();
    }


    void Update()
    {
        if (_chaserRef.currentState.stateName == STATE.Alert)
        {
            _chaserAnimator.SetFloat("speedPercentage", 0f);
        }
        if (_chaserRef.currentState.stateName == STATE.Patrol)
        {
            _chaserAnimator.SetFloat("speedPercentage", 0.5f);
			_audio.clip = Gruñido;

        }
        if(_chaserRef.currentState.stateName == STATE.Attack)
        {
            _chaserAnimator.SetFloat("speedPercentage", 1f);
			//audio.clip = arañazo;

        } 
		_currentTime += Time.deltaTime;
		if (_currentTime > 3.0f) {
			_currentTime = 0.0f;
			_audio.Play ();
		}

		
    }
}
