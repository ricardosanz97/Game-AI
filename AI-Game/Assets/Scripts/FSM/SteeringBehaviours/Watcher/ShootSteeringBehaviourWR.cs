using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootSteeringBehaviourWR : SteeringBehaviour
{
	public  int DistanceShoot;
	public PlayerController player;
	public float Cadencia;
	float timer;
	//AudioSource pum;
	//Light gunlight;

	public override void Act()
	{
		Debug.Log("PUM PUM PUM");
		//pum = GetComponent<AudioSource>;
	//	gunlight = GetComponent<Light>;
	}
	public  void Update()
	{
		var Distancia = Vector3.Distance (transform.position, player.transform.position);

		if (Distancia <= DistanceShoot  && Cadencia > 0.5f) {

			transform.LookAt (player.transform.position);
			Disparar ();

		}
		Cadencia += Time.deltaTime;
	}

	public void Disparar()
	{/*
		Cadencia = Of;

		pum.Play ();
		gunlight.enabled = true;

		//Restar vida al player.

*/
	}

}


