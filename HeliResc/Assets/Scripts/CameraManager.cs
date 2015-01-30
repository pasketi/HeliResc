using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {

	public GameObject copter;
	public float deadZone = 300f;
	public float dampTime = 0.15f;

	private Vector3 velocity = Vector3.zero;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (Camera.main.WorldToScreenPoint(copter.transform.position).x >= Screen.width - deadZone || Camera.main.WorldToScreenPoint(copter.transform.position).x <= deadZone){
			Vector3 destination = new Vector3(copter.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
			gameObject.transform.position = destination; 
		}
	}
}

/*
Vector3 point = camera.WorldToViewportPoint(target.position);
Vector3 delta = target.position - camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
Vector3 destination = transform.position + delta;
transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
*/