using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

public class CameraSelector : MonoBehaviour
{

	public Camera GameCamera;
	public Camera DebugCamera;
	public float CameraMoveSpeed = 20;
	public float MinOrthographicSize = 20;
	public float MaxOrthographicSize = 50;
	public float CameraZoomSpeed = 0.5f;
	
	private Camera _activeCamera;

	private void Start()
	{
		CameraZoomSpeed = MaxOrthographicSize - MinOrthographicSize;
		_activeCamera = GameCamera;
		GameCamera.enabled = true;
		DebugCamera.enabled = false;
	}

	private void OnGUI()
	{
		if (GUILayout.Button("Next Camera"))
		{
			if (_activeCamera == GameCamera)
			{
				_activeCamera = DebugCamera;
				GameCamera.enabled = false;
				DebugCamera.enabled = true;
				HUDManager.I.enabled = false;
			}
			else
			{
				HUDManager.I.enabled = true;
				_activeCamera = GameCamera;
				DebugCamera.enabled = false;
				GameCamera.enabled = true;
			}
				
		}
	}

	private void Update()
	{
		if (_activeCamera == DebugCamera)
		{
			float zoom = Input.GetAxis("Mouse ScrollWheel");
			
			if (zoom < 0 || zoom > 0) // back
				DebugCamera.orthographicSize += CameraZoomSpeed * zoom;

			DebugCamera.orthographicSize = Mathf.Clamp(DebugCamera.orthographicSize, MinOrthographicSize, MaxOrthographicSize);
			
			float h = Input.GetAxisRaw("Horizontal");
			float v = Input.GetAxisRaw("Vertical");
			Vector3 input = new Vector3(h,0.0f,v);
			DebugCamera.transform.SetPositionAndRotation(DebugCamera.transform.position + input * CameraMoveSpeed * Time.deltaTime,Quaternion.Euler(new Vector3(90,0,0)));
		}
	}
}
