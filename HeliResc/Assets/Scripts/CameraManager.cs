using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {

	private Vector3 cameraOriginal, target, zero = Vector3.zero;
	private float deadZonePixels, targetX, targetY;

	public GameObject copter;
	public float 	deadZonePercent = 0.2f, 
					dampTime = 2f, 
					maxSpeed = 20f,
					maxY = 30f;

	// Use this for initialization
	void Start () {
		cameraOriginal = Camera.main.transform.position;
		deadZonePixels = Screen.width * deadZonePercent;
	}
	
	// Update is called once per frame
	void Update () {

		if (copter != null && copter.transform.position.y <= cameraOriginal.y*2){
			targetY = cameraOriginal.y;
			target = new Vector3(targetX, targetY, cameraOriginal.z);
		} else if (copter != null && (Camera.main.WorldToScreenPoint(copter.transform.position).y >= Screen.height - Screen.height*(deadZonePercent/2) || Camera.main.WorldToScreenPoint(copter.transform.position).y <= Screen.height*deadZonePercent*2.5f)){
			targetY = copter.transform.position.y;
			if (targetY > maxY) targetY = maxY;
			target = new Vector3(targetX, targetY, cameraOriginal.z);
		}
		if (copter != null && (Camera.main.WorldToScreenPoint(copter.transform.position).x >= Screen.width - deadZonePixels || Camera.main.WorldToScreenPoint(copter.transform.position).x <= deadZonePixels)){
			targetX = copter.transform.position.x;
			target = new Vector3(targetX, targetY, cameraOriginal.z);
		}

		if (Camera.main.transform.position.x != target.x || Camera.main.transform.position.y != target.y) gameObject.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, target, ref zero, dampTime, maxSpeed); 
	}
}