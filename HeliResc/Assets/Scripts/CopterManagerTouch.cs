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
	private int rotationID1 = 255, rotationID2 = 255, powerID = 255, copterID = 255;

	// Public values
	public GameObject indicatorRect, hookPrefab, hookAnchor, brokenCopter;
	public bool isHookDead = false, isKill = false;
	public float 	maxTilt = 75f, 
					tiltSpeed = 50f, 
					returnSpeed = 5f,
					holdTime = 0.25f,
					rotationSensitivity = 0.5f,
					powerSensitivity = 0.5f,
					minPower = 0f,
					maxPower = 120f,
					currentPower = 75f,
					flyingAltitude = 4f, 
					maxVelocity = 3f,
					cargoMass = 0f,
					hookDistance = 1.5f,
					reelSpeed = 0.05f;

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

		// Control system
		if (Input.touchCount > 0) {
			foreach (Touch touch in Input.touches){

				//Copter press and Joystick initialization since both cannot happen during the same frame
				if(touch.phase == TouchPhase.Began && 
				   gameObject.collider2D == Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(touch.position)) && !isHookDead){
					//copterID = touch.fingerId;
					Debug.Log("Copter touched @ " + Time.time);
					if (isHookDown){
						isHookDown = false;
					} else {
						isHookDown = true;
					}
				}
				if (touch.phase == TouchPhase.Began) {
					currentAngle = copterAngle;
					if (rotationID1 == 255) {
						rotationID1 = touch.fingerId;
						Debug.Log ("Joystick 1 Initialized @ " + Time.time);
					} else if (rotationID2 == 255) {
						rotationID2 = touch.fingerId;
						Debug.Log ("Joystick 2 Initialized @ " + Time.time);
					}
				}

				//Joystick moved (JOYSTICK CONTROLS ROTATION AND POWER MANAGEMENT 
				if ((touch.fingerId == rotationID1 || touch.fingerId == rotationID2) && touch.phase == TouchPhase.Moved) {
					//Rotation
					if ((currentAngle < maxTilt || touch.deltaPosition.x < 0f) || 
					    (currentAngle > 360f-maxTilt || touch.deltaPosition.x > 0f)) currentAngle -= touch.deltaPosition.x * rotationSensitivity;

					if (currentAngle < 0f) currentAngle += 360f;
					else if (currentAngle > 360f) currentAngle -= 360f;
					
					if (currentAngle > maxTilt && currentAngle < 180f) currentAngle = maxTilt;
					else if (currentAngle < 360f - maxTilt && currentAngle > 180f) currentAngle = 360f - maxTilt;

					//Power Management
					if (currentPower <= maxPower || currentPower >= minPower) {
						currentPower += (touch.deltaPosition.y/Screen.height)*(maxPower*powerSensitivity);
					}

					//Debug.Log ("Joystick moved " + currentAngle);
				}

				//Control destruction
				if (touch.phase == TouchPhase.Ended) {
					if (touch.fingerId == rotationID1) {
						rotationID1 = 255;
						Debug.Log ("Joystick #1 Destroyed @ " + Time.time);
					} else if (touch.fingerId == rotationID2) {
						rotationID2 = 255;
						Debug.Log ("Joystick #2 Destroyed @ " + Time.time); 
					} /*else if (touch.fingerId == copterID) {
						Debug.Log ("Copter touch ended @ " + Time.time);
						copterID = 255;
					}*/
					if (rotationID1 == 255 && rotationID2 == 255) {
						currentAngle = 0f;
						tempHoldTime = 0f;
					}
				}
			}

		}

		//------------------------------------------------------------------------------------------------------------------------------------------

		// Helicopter angle management

		if (copterAngle != 0f && rotationID1 == 255) { // Return to 0°
			if (copterAngle > 180f) {
				gameObject.transform.Rotate (new Vector3 (0f, 0f, returnSpeed * Time.deltaTime * (360f - copterAngle) * persistence));
			} else if (copterAngle < 180f) {
				gameObject.transform.Rotate (new Vector3 (0f, 0f, -(returnSpeed * Time.deltaTime) * copterAngle * persistence));
			}
		} else if (copterAngle != currentAngle && rotationID1 != 255) { // Turn to currentAngle
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
		if (currentAngle > 180f && rotationID1 != 255) gameObject.transform.localScale = new Vector3(copterScale, gameObject.transform.localScale.y); 
		else if (currentAngle < 180f && rotationID1 != 255) gameObject.transform.localScale = new Vector3(-copterScale, gameObject.transform.localScale.y);

		// END INPUT ------------------------------------------------------------------------------------------------------------------------------------------

		copterBody.AddForce (gameObject.transform.up * (currentPower*100) * Time.deltaTime);

		if (isHookDown && hook == null && !isHookDead) {
			once = true;
			hook = Instantiate (hookPrefab, gameObject.transform.position + new Vector3 (0f, -0.3f), Quaternion.identity) as GameObject;
			hookJoint.enabled = true;
			hookJoint.distance = hookDistance;
			hookJoint.connectedBody = hook.rigidbody2D;
		} else if (!isHookDown && once && Vector2.Distance (hook.transform.position, hookAnchor.transform.position) < 0.1 && !isHookDead) {
			manager.cargoHookedCrates (hook);
			Destroy (hook);
			once = false;
		} else if (!isHookDown && hook != null && !isHookDead) {
			hookJoint.distance -= reelSpeed;
		} else if (isHookDown && hookJoint.distance != hookDistance) {
			hookJoint.distance = hookDistance;
		}
		if (!isHookDead && isHookDown && Vector2.Distance (hook.transform.position, hookAnchor.transform.position) > hookDistance*2) {
			killHook();
		} 

		if (tempHoldTime != holdTime) {
			tempHoldTime += Time.deltaTime;
			persistence = tempHoldTime/holdTime;
		}
		if (tempHoldTime > holdTime) {
			persistence = 1f;
			tempHoldTime = holdTime;
		}

		if ((gameObject.transform.position.y) < manager.getWaterLevel()){
			manager.Reset();
		}

		powerIndPosition = new Vector2(0f, ((Screen.height*manager.uiLiftPowerDeadZone)*(maxPower-(2*currentPower)+minPower)+(currentPower-minPower)*Screen.height)/(maxPower-minPower));
		powerIndRect.anchoredPosition = new Vector2(0, powerIndPosition.y);

		if (isKill) kill();
	}

	private void killHook () {
		hookJoint.enabled = false;
		hook.GetComponent<DestroyHookOverTime>().doom();
		hook = null;
		isHookDead = true;
	}

	public void kill() {
		if (hook != null)
			killHook ();
		GameObject newCopter = (GameObject) Instantiate(brokenCopter, transform.position, Quaternion.identity);
		Rigidbody2D[] parts = newCopter.GetComponentsInChildren<Rigidbody2D>();
		foreach(Rigidbody2D part in parts){
			part.velocity += gameObject.rigidbody2D.velocity;
			part.angularVelocity += gameObject.rigidbody2D.angularVelocity;
		}
		Destroy (gameObject);
	}
}
