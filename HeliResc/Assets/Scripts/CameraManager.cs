using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {
	private Vector3 cameraOriginal, target, zero = Vector3.zero;
	private float deadZonePixels, targetX, targetY;
	
	private GameObject copter;
	public bool hasBounds = false;
	public float 	deadZonePercent = 0.2f, 
	dampTime = 2f, //good value is 2f
	maxSpeed = 20f, //good value is 20f
	maxY = 30f,
	mapBoundsLeft = 0f,
	mapBoundsRight = 0f;
	
	private float slowDamp = 2.1f, //camre moves slow if copter moves slow
	fastDamp = 0.35f, //short dump time needed when copter takes fast action
	fastVelosity = 4.5f, //velocity that is concidered to be fast
	cameraFront = 3f; //how much camera is forvard from copter's x positon
	private bool cameraMovingRight = false, //true if the copter and the camera is moving the same direction, false if copter change its flying direction.
	cameraMovingLeft = false;
	private Rigidbody2D copterRb;		//Reference to the copters rigidbody to access its velocity
	
	// Use this for initialization
	void Start () {
		if (copter == null) copter = GameObject.Find("Copter");
		cameraOriginal = Camera.main.transform.position;
		deadZonePixels = Screen.width * deadZonePercent;
		
		copterRb = copter.GetComponent<Rigidbody2D> ();
		targetX = copterRb.transform.position.x + cameraFront; //find copter when level starts
		//targetY = copterRb.transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
		
		
		
		Vector2 vel = copterRb.velocity;
		
		if (copter != null && copter.transform.position.y <= cameraOriginal.y*2){
			targetY = cameraOriginal.y;
			target = new Vector3(targetX, targetY, cameraOriginal.z);
		} else if (copter != null && (Camera.main.WorldToScreenPoint(copter.transform.position).y >= Screen.height - Screen.height*(deadZonePercent/2) || Camera.main.WorldToScreenPoint(copter.transform.position).y <= Screen.height*deadZonePercent*2.5f)){
			targetY = copter.transform.position.y;
			if (targetY > maxY) targetY = maxY;
			target = new Vector3(targetX, targetY, cameraOriginal.z);
		}
		//does copter enter in edge zone:
		//left edge zone?
		if (copter != null && (Camera.main.WorldToScreenPoint (copter.transform.position).x <= deadZonePixels)) {
			cameraMovingLeft = true;
		}
		//Right edge zone?
		if (copter != null && (Camera.main.WorldToScreenPoint (copter.transform.position).x >= (Screen.width - deadZonePixels))) {
			cameraMovingRight = true;
		}
		//Should camera be moved?
		if (copter != null && (cameraMovingLeft == true || cameraMovingRight == true)){
			if (cameraMovingLeft == true) { 
				targetX = (copter.transform.position.x - cameraFront); //Move camera so that player can see move left
			}else{
				targetX = (copter.transform.position.x + cameraFront); //Move camera so that player can see move right
			}
			//targetX = copter.transform.position.x; //move camera x position to copter's x position
			if (hasBounds && targetX < mapBoundsLeft) targetX = mapBoundsLeft;
			else if (hasBounds && targetX > mapBoundsRight) targetX = mapBoundsRight;
			target = new Vector3(targetX, targetY, cameraOriginal.z);
			
			//Has copter changed its direction?
			if (vel.x < 0 && cameraMovingRight == true) {
				cameraMovingRight = false; //Stop moving wrong way
			}
			if (vel.x > 0 && cameraMovingLeft == true) {
				cameraMovingLeft = false; //Stop moving wrong way
			}
			//Should camera be moved fast?
			if (Mathf.Abs(vel.x) > fastVelosity) {
				//Move camera fast to get to the copter
				dampTime = fastDamp;
			}else{
				//Everyting is fine. Set normal speed.
				dampTime = slowDamp;
			}
		}
		
		if (Camera.main.transform.position.x != target.x || Camera.main.transform.position.y != target.y) gameObject.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, target, ref zero, dampTime, maxSpeed);
		//gameObject.transform.position = Vector3.Lerp (gameObject.transform.position, target, 3.5f * Time.deltaTime);
		//gameObject.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, target, ref zero, dampTime, maxSpeed); 
	}


}