using UnityEngine;
using System.Collections;

public class CameraManager_ : MonoBehaviour {
	
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
	cameraFront = 4f; //how much camera is forvard from copter's x positon
	private bool cameraMovingRight = false, //true if the copter and the camera is moving the same direction, false if copter change its flying direction.
	cameraMovingLeft = false;
	private Rigidbody2D copterRb;		//Reference to the copters rigidbody to access its velocity
	
	// Use this for initialization
	void Start () {
		if (copter == null) copter = GameObject.Find("Copter");
		cameraOriginal = Camera.main.transform.position;
		deadZonePixels = Screen.width * deadZonePercent;

		copterRb = copter.GetComponent<Rigidbody2D> ();
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






//using UnityEngine;
//using System.Collections;
//
//public class CameraManager : MonoBehaviour {
//
//	public Transform test;
//
//	public Vector2 minVelocity;			//Minimum velocity of the copter to start move the camera
//	public Vector2 maxVelocity;			//Maximum velocity of the copter. The camera moves with full speed when copter goes faster than this value 
//	
//    public float maxY = 30f;			//The value to stop following the copter
//	public float xBound;				//The value the copter can move to without moving the camera when going slower than minVelocity. Value is percentage of the screen width
//	public float yBound;
//	public float xDistance;				//The max value in unity units to move the camera when copter is going over minVelocity
//    public float yDistance;
//    public float acceleration = 10;
//    
//
//	private Vector3 cameraOriginal;		//Cameras position at the start
//	private Vector3 target;				//Target position to move the camera to
//
//	private float targetX;
//	private float targetY;
//
//	//private float cameraSpeed = 30;
//
//	private Transform copter;			//Reference to the copter transform to access its position
//	private Rigidbody2D copterRb;		//Reference to the copters rigidbody to access its velocity
//	private Transform _transform;		//Reference to own transform
//
//	private Vector3 zero = Vector3.zero;
//
//
//					//mapBoundsLeft = 0f,
//					//mapBoundsRight = 0f;
//
//	// Use this for initialization
//	void Start () {
//
//		_transform = transform;
//		GameObject go = GameObject.Find ("Copter");
//		copter = go.transform;
//		copterRb = go.GetComponent<Rigidbody2D> ();
//
//
//		//Calculate the width and height of the screen in unity units
//		float width = Vector3.Distance (Camera.main.ScreenToWorldPoint (Vector3.zero), Camera.main.ScreenToWorldPoint (Vector3.right * Screen.width));
//		float height = Vector3.Distance (Camera.main.ScreenToWorldPoint (Vector3.zero), Camera.main.ScreenToWorldPoint (Vector3.up * Screen.height));
//
//		xBound = (width/2) - (xBound * width);
//		yBound = (height/2) - (yBound * height);
//
//		cameraOriginal = Camera.main.transform.position;
//		//deadZonePixels = Screen.width * deadZonePercent;
//	}
//	
//	// Update is called once per frame
//	void Update () {
//
//		Vector2 vel = copterRb.velocity;
//
//		//Check the copter x velocity to adjust the camera position accordingly
//		if (vel.x >= minVelocity.x || vel.x <= -minVelocity.x) {
//			float x = Mathf.Sign(vel.x) * -xDistance;
//			if(Mathf.Abs(vel.x) < maxVelocity.x)
//				x *= Mathf.Clamp01 (Mathf.Abs (vel.x) / maxVelocity.x);
//			targetX = copter.position.x - x;
//		} else if ((copter.position.x >= _transform.position.x + xBound)
//		        || (copter.position.x < _transform.position.x - xBound)) {
//			targetX = copter.position.x;
//		}
//
//        if (vel.y >= minVelocity.y || vel.y <= -minVelocity.y) {
//            float y = Mathf.Sign(vel.y) * -yDistance;
//            if (Mathf.Abs(vel.y) < maxVelocity.y)
//                y *= Mathf.Clamp01(Mathf.Abs(vel.y) / maxVelocity.y);
//            targetY = copter.position.y - y;
//        } 
//        else if ((copter.position.y >= _transform.position.y + yBound) 
//                || (copter.position.y < _transform.position.y - yBound)) {
//            targetY = copter.position.y;
//        }
//
//        if(targetY < cameraOriginal.y) {
//            targetY = cameraOriginal.y;
//        }
//
//		target = new Vector3 (targetX, targetY, -10);
//
//		if (test != null) {
//			test.position = new Vector3(target.x, target.y);
//		}
//
//        Debug.Log("Distance to target: " + (Vector3.Distance(target, _transform.position) < .5f));
//
//        if (Camera.main.transform.position.x != target.x || Camera.main.transform.position.y != target.y) {
//            if(Vector3.Distance(target, _transform.position) > .5f)
//                _transform.Translate((target - _transform.position).normalized * acceleration * Time.deltaTime);
//        }
//
//		/*if (copter.position.y <= cameraOriginal.y*2){
//			targetY = cameraOriginal.y;
//			target = new Vector3(targetX, targetY, cameraOriginal.z);
//		} else if ((Camera.main.WorldToScreenPoint(copter.position).y >= Screen.height - Screen.height*(deadZonePercent/2) || Camera.main.WorldToScreenPoint(copter.position).y <= Screen.height*deadZonePercent*2.5f)){
//			targetY = copter.position.y;
//			if (targetY > maxY) targetY = maxY;
//			target = new Vector3(targetX, targetY, cameraOriginal.z);
//		}
//		if ((Camera.main.WorldToScreenPoint(copter.position).x >= Screen.width - deadZonePixels || Camera.main.WorldToScreenPoint(copter.position).x <= deadZonePixels)){
//			targetX = copter.position.x;
//			if (hasBounds && targetX < mapBoundsLeft) targetX = mapBoundsLeft;
//			else if (hasBounds && targetX > mapBoundsRight) targetX = mapBoundsRight;
//			target = new Vector3(targetX, targetY, cameraOriginal.z);
//		}
//*/
//        
//	}
//}