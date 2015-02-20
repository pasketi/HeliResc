using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CopterManagerTouch : MonoBehaviour {

	//USE THIS TO SWAP BETWEEN THE CONTROL SYSTEMS
	// true == Erte && Panu version
	// false == Cifu && Henri version (sorry)
	public bool controlSystem = true;

	// Private stuff
	private Rigidbody2D copterBody;
	private DistanceJoint2D hookJoint;
	private bool isHookDown = true, once = false;
	private Vector2 powerIndPosition;
	private float 	currentAngle = 0f,  
					copterAngle = 0f, 
					copterScale = 0f, 
					persistence = 1f,
					tempHoldTime = 0f, 
					controlSmoothZone = 1f;
	private GameObject hook;
	private RectTransform powerIndRect;
	private LevelManager manager;
	private int rotationID = 255, powerID = 255, copterID = 255;

	// Public values
	public GameObject indicatorRect, hookPrefab, hookAnchor;
	public float 	maxTilt = 75f, 
					tiltSpeed = 50f, 
					returnSpeed = 5f,
					holdTime = 0.25f,
					rotationSensitivity = 0.5f,
					minPower = 60f,
					maxPower = 120f,
					currentPower = 75f,
					flyingAltitude = 4f, 
					maxVelocity = 3f,
					cargoMass = 0f;

	public void pickUpCrate (float crateMass) {
		gameObject.rigidbody2D.mass += crateMass;
		cargoMass += crateMass;
	}

	public void dropOneCrate (float crateMass) {
		gameObject.rigidbody2D.mass -= crateMass;
		cargoMass -= crateMass;
	}

	public void dropAllCrates () {
		gameObject.rigidbody2D.mass -= cargoMass;
		cargoMass = 0f;
	}
	
	// Use this for initialization
	void Start () {
		manager = (LevelManager) GameObject.Find("LevelManagerO").GetComponent(typeof(LevelManager));
		copterBody = gameObject.GetComponent<Rigidbody2D>();
		powerIndRect = indicatorRect.GetComponent<RectTransform> ();
		hookJoint = GetComponent<DistanceJoint2D> ();
		hookJoint.anchor = hookAnchor.transform.localPosition;
		copterScale = gameObject.transform.localScale.x;
		tempHoldTime = holdTime;
		powerIndPosition = new Vector2(0f, ((Screen.height*manager.uiLiftPowerDeadZone)*(maxPower-(2*currentPower)+minPower)+(currentPower-minPower)*Screen.height)/(maxPower-minPower));
	}

	//Update is called before void Update();
	void FixedUpdate () {
		copterAngle = gameObject.transform.eulerAngles.z;
	}

	// Update is called once per frame
	void Update () {

		// for powermanagement in editor
		if (currentPower < minPower) currentPower = minPower;
		if (currentPower > maxPower) currentPower = maxPower;

		if (minPower > maxPower) minPower = maxPower;
		if (maxPower < minPower) maxPower = minPower;


		// START INPUT ------------------------------------------------------------------------------------------------------------------------------------------

		/* NEW CONTROL LOGIC
		 * -----------------
		 * VARIABLES FOR ROTATION
		 * rootAngle = the angle for joystick 0°
		 * currentAngle = the angle for current joystick position
		 * joystickPosition = current screen position of joystick
		 * 
		 * VARIABLES FOR POWER MANAGEMENT
		 * minPower = minimum lift power for the copter (Enough to decend slowly with empty cargo)
		 * maxPower = maximum lift power for the copter
		 * currentPower = current position of power meter
		 * 
		 * Cycle through every touch
		 * 
		 * 		ROTATION CONTROL
		 * 		if touchPhase has began on horizontal control area
		 * 			Set the currentAngle to current copter angle
		 * 			Create joystick on touch position
		 * 		if touchPhase on rotation control area stationary or moved
		 * 			Get X on the joystick and set currentAngle as X
		 * 
		 * 		POWER CONTROL
		 * 		if touchPhase has began on power control area
		 * 			Set the anchorpoint to touch position
		 * 		if power control touchPhase has moved
		 * 			Set currentPower as follows: currentPower = getPowerFromMovement(minPower, maxPower, touch.deltaPosition)
		 * 
		 * 		if ANY kind of touchPhase ended
		 * 			if Rotation control
		 * 				set currentAngle to 0f, so the copter returns
		 * 				Destroy joystick
		 * 			if Power control
		 * 				
		 * 
		 * Automatically
		 * 		Set the copter angle to currentAngle if possible using modified automatic copter return to 0° algorithm
		 * 		Give copter lift according to currentPower
		 */

		// Control system
		if (Input.touchCount > 0) {
			foreach (Touch touch in Input.touches){

				//Copter press and Joystick initialization since both cannot happen during the same frame
				if(touch.phase == TouchPhase.Began && 
				   gameObject.collider2D == Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(touch.position))){
					copterID = touch.fingerId;
					Debug.Log("Copter touched @ " + Time.time);
					if (isHookDown){
						isHookDown = false;
					} else {
						isHookDown = true;
					}
				} else if (touch.position.x > Screen.width * manager.uiLiftPowerWidth && touch.phase == TouchPhase.Began) {
					currentAngle = copterAngle;
					rotationID = touch.fingerId;
					Debug.Log ("Joystick Initialized @ " + Time.time);
				}

				//Joystick moved
				if (touch.fingerId == rotationID && touch.phase == TouchPhase.Moved) {
					if ((currentAngle < maxTilt || touch.deltaPosition.x < 0f) || 
					    (currentAngle > 360f-maxTilt || touch.deltaPosition.x > 0f)) currentAngle -= touch.deltaPosition.x * rotationSensitivity;

					if (currentAngle < 0f) currentAngle += 360f;
					else if (currentAngle > 360f) currentAngle -= 360f;
					
					if (currentAngle > maxTilt && currentAngle < 180f) currentAngle = maxTilt;
					else if (currentAngle < 360f - maxTilt && currentAngle > 180f) currentAngle = 360f - maxTilt;

					//Debug.Log ("Joystick moved " + currentAngle);
				}

				//Power initialization
				if (touch.position.x < Screen.width * manager.uiLiftPowerWidth && touch.phase == TouchPhase.Began) {
					powerID = touch.fingerId;
					Debug.Log ("Power finger initialized @ " + Time.time);
				}

				//Power finger management
				if (touch.fingerId == powerID && (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)) {
					currentPower = (((maxPower + minPower) * (Screen.height*manager.uiLiftPowerDeadZone)) - ((maxPower-minPower) * touch.position.y) - minPower*Screen.height) / ((2 * (Screen.height*manager.uiLiftPowerDeadZone)) - Screen.height);
					if (touch.position.y < Screen.height - Screen.height*manager.uiLiftPowerDeadZone || touch.position.y > Screen.height*manager.uiLiftPowerDeadZone)
						powerIndPosition = new Vector2(0f, touch.position.y);
					//Debug.Log ("Power finger check " + touch.deltaPosition);
				}

				//Control destruction
				if (touch.phase == TouchPhase.Ended) {
					if (touch.fingerId == rotationID) {
						currentAngle = 0f;
						tempHoldTime = 0f;
						rotationID = 255;
						Debug.Log ("Joystick Destroyed @ " + Time.time);
					} else if (touch.fingerId == powerID) {
						Debug.Log ("Power Control disengaged @ " + Time.time);
						powerID = 255;
					} else if (touch.fingerId == copterID) {
						Debug.Log ("Copter touch ended @ " + Time.time);
						copterID = 255;
					}
				}
			}

		}

		//------------------------------------------------------------------------------------------------------------------------------------------
		/*
		// Left rotate
		if (Input.touchCount > 1){}
		else if (Input.touchCount > 0 && Input.GetTouch(0).position.x < Screen.width/2 && Input.GetTouch(0).position.x > manager.uiLiftPowerWidth){
			//  happens the first frame
			if (lastMovementRight != false){
				gameObject.transform.localScale = new Vector3(-gameObject.transform.localScale.x, gameObject.transform.localScale.y);
				lastMovementRight = false;
			}

			// What happens every frame
			if (gameObject.transform.eulerAngles.z >= maxTilt 
			    && gameObject.transform.eulerAngles.z <= 180f) {} else {
				gameObject.transform.Rotate(new Vector3(0f, 0f, tiltSpeed*Time.deltaTime));
			}
		}

		// Right rotate
		if (Input.touchCount > 1){}
		else if (Input.touchCount > 0 && Input.GetTouch(0).position.x > Screen.width/2 && Input.GetTouch(0).position.x < Screen.width - manager.uiLiftPowerWidth) {
			// What happens the first frame
			if (lastMovementRight != true) {
				gameObject.transform.localScale = new Vector3(-gameObject.transform.localScale.x, gameObject.transform.localScale.y);
				lastMovementRight = true;
			}

			// What happens every frame
			if (gameObject.transform.eulerAngles.z <= 360f-maxTilt 
			    && gameObject.transform.eulerAngles.z >= 180f) {} else {
				gameObject.transform.Rotate (new Vector3 (0f, 0f, -tiltSpeed * Time.deltaTime));
			}
		}
		 */

		// Helicopter angle management

		if (copterAngle != 0f && rotationID == 255) { // Return to 0°
			if (copterAngle > 180f) {
				gameObject.transform.Rotate (new Vector3 (0f, 0f, returnSpeed * Time.deltaTime * (360f - copterAngle) * persistence));
			} else if (copterAngle < 180f) {
				gameObject.transform.Rotate (new Vector3 (0f, 0f, -(returnSpeed * Time.deltaTime) * copterAngle * persistence));
			}
		} else if (copterAngle != currentAngle && rotationID != 255) { // Turn to currentAngle
			if (currentAngle < 180f) {
				if (copterAngle > 180f) {
					// Rotate CCW
					gameObject.transform.Rotate(new Vector3(0f, 0f, tiltSpeed * Time.deltaTime));
				} else if (copterAngle < 180f) {
					if (copterAngle < currentAngle - controlSmoothZone) {
						// Rotate CCW
						gameObject.transform.Rotate (new Vector3 (0f, 0f, tiltSpeed * Time.deltaTime));
					} else if (copterAngle > currentAngle + controlSmoothZone) {
						// Rotate CW
						gameObject.transform.Rotate(new Vector3(0f, 0f, -tiltSpeed * Time.deltaTime));
					}
				}
			} else if (currentAngle > 180f) {
				if (copterAngle < 180f) {
					// Rotate CW
					gameObject.transform.Rotate (new Vector3 (0f, 0f, -tiltSpeed * Time.deltaTime));
				} else if (copterAngle > 180f) {
					if (copterAngle < currentAngle - controlSmoothZone) {
						// Rotate CCW
						gameObject.transform.Rotate (new Vector3 (0f, 0f, tiltSpeed * Time.deltaTime));
					} else if (copterAngle > currentAngle + controlSmoothZone) {
						// Rotate CW
						gameObject.transform.Rotate(new Vector3(0f, 0f, -tiltSpeed * Time.deltaTime));
					}
				}
			}
		}

		//Copter direction mangement
		if (currentAngle > 180f && rotationID != 255) gameObject.transform.localScale = new Vector3(copterScale, gameObject.transform.localScale.y); 
		else if (currentAngle < 180f && rotationID != 255) gameObject.transform.localScale = new Vector3(-copterScale, gameObject.transform.localScale.y);

		// END INPUT ------------------------------------------------------------------------------------------------------------------------------------------

		copterBody.AddForce (gameObject.transform.up * (currentPower*100) * Time.deltaTime);

		if (isHookDown && hook == null) {
			once = true;
			hook = Instantiate (hookPrefab, gameObject.transform.position + new Vector3 (0f, -0.3f), Quaternion.identity) as GameObject;
			hookJoint.connectedBody = hook.rigidbody2D;
		} else if (!isHookDown && once) {
			manager.cargoHookedCrates(hook);
			Destroy(hook);
			once = false;
		}

		if (tempHoldTime != holdTime) {
			tempHoldTime += Time.deltaTime;
			persistence = tempHoldTime/holdTime;
		}
		if (tempHoldTime > holdTime) {
			persistence = 1f;
			tempHoldTime = holdTime;
		}

		powerIndPosition = new Vector2(0f, ((Screen.height*manager.uiLiftPowerDeadZone)*(maxPower-(2*currentPower)+minPower)+(currentPower-minPower)*Screen.height)/(maxPower-minPower));
		powerIndRect.anchoredPosition = new Vector2(0, powerIndPosition.y);
	}
}
