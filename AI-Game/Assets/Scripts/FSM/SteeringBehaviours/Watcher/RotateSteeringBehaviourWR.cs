using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSteeringBehaviourWR : SteeringBehaviour
{
	private Rigidbody rb;
	private PlayerController controller;
	public float turnSpeed = 50f;
	public override void Act()
	{
		rb = GetComponent<Rigidbody>();
		controller = base.player.GetComponent<PlayerController>();
		ChaserRotation ();

	}



	public void ChaserRotation()
	{

		Debug.Log("WatcherRotation");
		if (rb.transform.rotation.y <= 90) {
			
			rb.transform.Rotate (Vector3.up, turnSpeed * Time.deltaTime);
			ChaserRotation ();
		}
		else if (rb.transform.rotation.y >= -90) {
			rb.transform.Rotate (Vector3.up, -turnSpeed * Time.deltaTime);
			ChaserRotation ();
		}
	}
}

