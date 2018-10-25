using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour {

    public float mouseSensitivity = 10f;
    public Transform target;
    public float distanceFromTarget = 4f;
    public Vector2 pitchMinMax = new Vector2(-40, 85);
    public float rotationSmoothTime = 0.1f;

    private float yaw; //Horizontal camera movement
    private float pitch; //Vertical camera movement
    private Vector3 rotationSmoothVelocity;
    private Vector3 currentRotation;

	// Update is called once per frame
	void LateUpdate () {
        CameraRotation();
	}

    void CameraRotation()
    {
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

        Vector3 targetRotation = new Vector3(pitch, yaw);
        currentRotation = Vector3.SmoothDamp(currentRotation, targetRotation, ref rotationSmoothVelocity, rotationSmoothTime);

        transform.eulerAngles = currentRotation;

        transform.position = target.position - transform.forward * distanceFromTarget;
    }
}
