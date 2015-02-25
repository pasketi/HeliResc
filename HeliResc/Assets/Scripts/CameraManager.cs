using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {

	private Vector3 cameraOriginal, target, zero = Vector3.zero;
	private float deadZonePixels;

	public GameObject copter;
	public float deadZonePercent = 0.2f;
	public float dampTime = 0.15f;
	public float maxSpeed = 1f;

	// Use this for initialization
	void Start () {
		cameraOriginal = Camera.main.transform.position;
		deadZonePixels = Screen.width * deadZonePercent;
	}
	
	// Update is called once per frame
	void Update () {
		//maxSpeed = copter.rigidbody2D.velocity.x;
		if (copter != null && (Camera.main.WorldToScreenPoint(copter.transform.position).x >= Screen.width - deadZonePixels || Camera.main.WorldToScreenPoint(copter.transform.position).x <= deadZonePixels)){
			target = new Vector3(copter.transform.position.x, cameraOriginal.y, cameraOriginal.z);
		}
		if (Camera.main.transform.position.x != target.x) gameObject.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, target, ref zero, dampTime, maxSpeed); 
	}
}