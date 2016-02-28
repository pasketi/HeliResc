using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {
	private Vector3 cameraOriginal, target, zero = Vector3.zero;
	private float deadZonePixels, targetX, targetY;
	
	private GameObject copter;
	public bool hasBounds = false;
	public float 	deadZonePercent = 0.25f, 
	dampTime = 2f, //good value is 2f
	maxSpeed = 20f, //good value is 20f
	maxY = 30f, //the camera can not get higher than this
	minY = 4f, //the camera can not get lower than this
	mapBoundsLeft = 0f,
	mapBoundsRight = 0f;
	
	private float slowDamp = 2.1f, //camre moves slow if copter moves slow
	fastDamp = 0.35f, //short dump time needed when copter takes fast action (old fastDamp = 0.35f)
	dampX = 2.1f, //demand for dump time time from x
	dampY = 2.1f, //demand for dump time time from y
	slowVelocity = 1f, //velocity that is concidered to be slow (use slowDump in lower speed)
	fastVelocity = 4.5f, //velocity that is concidered to be fast (use fastDump with higher speed)
	risingVelocity = 1f, //expected speed to rise the copter. If the copter is rising slowler that is propably meant to be hoovering
	cameraFront = 3.5f, //how much camera is forvard from copter's x positon
	cameraDownLook = 2.5f; //if the copter is moving down the camera looks further to see the sea or the ground earlier. 
	private bool cameraMovingRight = true, //true if the copter and the camera is moving the same direction, false if copter change its flying direction.
	cameraMovingLeft = false;
	private Rigidbody2D copterRb;		//Reference to the copters rigidbody to access its velocity
	
	// Use this for initialization
	void Start () {
		if (copter == null) copter = GameObject.Find("Copter");
		cameraOriginal = Camera.main.transform.position; //this is mainly to same camera z-position
		deadZonePixels = Screen.width * deadZonePercent;
		
		copterRb = copter.GetComponent<Rigidbody2D> ();
		targetX = copterRb.transform.position.x + cameraFront; //find copter when level starts
		//targetY = copterRb.transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
		
		
		
		Vector2 vel = copterRb.velocity;

		//camera do not follow the copter if the copter does not excist (eg. it is exploded)
		if (copter != null) {
			if (vel.y < 0) {
				//copter is moving down, so look down
				targetY = copter.transform.position.y - cameraDownLook;
			} else if (vel.y < risingVelocity){
				//copter is not moving down
				targetY = copter.transform.position.y - (cameraDownLook*(1-(vel.y/risingVelocity)));
			}else{
				//center copter to the screen by y-position
				targetY = copter.transform.position.y;
			}
			if (targetY > maxY)
				targetY = maxY; //do not move camera to the space, let us stay just on earth
			if (targetY < minY)
				targetY = minY; //no need to see deap sea fish while flying
			target = new Vector3 (targetX, targetY, cameraOriginal.z);
			/*
			 * THIS IS OLD CODE TO MANAGE CAMERA Y-POSITION
			if (copter.transform.position.y <= minY * 2) {
				targetY = cameraOriginal.y;
				target = new Vector3 (targetX, targetY, cameraOriginal.z);
			} else if ((Camera.main.WorldToScreenPoint (copter.transform.position).y >= Screen.height - Screen.height * (deadZonePercent / 2) || Camera.main.WorldToScreenPoint (copter.transform.position).y <= Screen.height * deadZonePercent * 2.5f)) {
				targetY = copter.transform.position.y;
				if (targetY > maxY)
					targetY = maxY; //do not move camera to the space, let us stay just on earth
				target = new Vector3 (targetX, targetY, cameraOriginal.z);
			}
			*/
		
			//does copter enter in edge zone:
			//left edge zone?
			if (Camera.main.WorldToScreenPoint (copter.transform.position).x <= deadZonePixels) {
				cameraMovingLeft = true;
			}
			//Right edge zone?
			if (Camera.main.WorldToScreenPoint (copter.transform.position).x >= (Screen.width - deadZonePixels)) {
				cameraMovingRight = true;
			}
			//Should camera be moved?
			if (cameraMovingLeft == true || cameraMovingRight == true) {
				if (cameraMovingLeft == true) { 
					targetX = (copter.transform.position.x - cameraFront); //Move camera so that player can see move left
				} else {
					targetX = (copter.transform.position.x + cameraFront); //Move camera so that player can see move right
				}
				//targetX = copter.transform.position.x; //move camera x position to copter's x position
				if (hasBounds && targetX < mapBoundsLeft)
					targetX = mapBoundsLeft;
				else if (hasBounds && targetX > mapBoundsRight)
					targetX = mapBoundsRight;
				target = new Vector3 (targetX, targetY, cameraOriginal.z);
			
				//Has copter changed its direction?
				if (vel.x < 0 && cameraMovingRight == true) {
					cameraMovingRight = false; //Stop moving wrong way
				}
				if (vel.x > 0 && cameraMovingLeft == true) {
					cameraMovingLeft = false; //Stop moving wrong way
				}
			}
				//Should the camera be moved fast?
			if (Mathf.Abs (vel.x) > slowVelocity) {
				if (Mathf.Abs (vel.x) > fastVelocity) {
					//Move camera fast to get to the copter
					dampX = fastDamp;
				} else {
					//copter is moving within mid range velocity
					//get suitable dampTime
					dampX = fastDamp + ((slowDamp - fastDamp) * (1f - ((Mathf.Abs (vel.x) - slowVelocity) / (fastVelocity - slowVelocity))));
				}
			} else {
				//the copter nearly moves. No need to hurry with the camera
				dampX = slowDamp;
			}
			//fit dampTime to y-velocity
			if (Mathf.Abs (vel.y) > fastVelocity*0.7f) {
				//Move camera fast to get to the copter
				dampY = fastDamp;

			} else {
				//copter is moving within mid range velocity
				//get suitable dampTime
				dampY = fastDamp + ((slowDamp - fastDamp) * (1f - (Mathf.Abs (vel.y) / fastVelocity*0.7f)));
			}

			if (dampX < dampY) {
				dampTime = dampX;
			}else{
				dampTime = dampY;
				//Debug.Log(dampTime+"the copter moves fast vertically");
			}
			
		}
		if (Camera.main.transform.position.x != target.x || Camera.main.transform.position.y != target.y) gameObject.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, target, ref zero, dampTime, maxSpeed);
		//gameObject.transform.position = Vector3.Lerp (gameObject.transform.position, target, 3.5f * Time.deltaTime);
		//gameObject.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, target, ref zero, dampTime, maxSpeed); 
	}


}