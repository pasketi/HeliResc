﻿using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {

	private float cameraOriginalY;

	public GameObject copter;
	public float deadZone = 300f;
	public float dampTime = 0.15f;

	// Use this for initialization
	void Start () {
		cameraOriginalY = Camera.main.transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
		if (copter != null && (Camera.main.WorldToScreenPoint(copter.transform.position).x >= Screen.width - deadZone || Camera.main.WorldToScreenPoint(copter.transform.position).x <= deadZone)){
			Vector3 destination = new Vector3(copter.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
			gameObject.transform.position = destination; 
		}
	}
}