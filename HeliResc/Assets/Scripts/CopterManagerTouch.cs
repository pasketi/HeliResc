using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CopterManagerTouch : MonoBehaviour {

	// Private stuff
	private Rigidbody2D copterBody;
	private DistanceJoint2D hookJoint;
	private bool once = false;
	private Vector2 powerIndPosition;
	private float 	currentAngle = 0f,  
					copterAngle = 0f, 
					copterScale = 0f, 
					persistence = 1f,
					tempHoldTime = 0f, 
					controlSmoothZone = 1f,
					currentPower;
	private GameObject hook;
	private RectTransform powerIndRect;
	private LevelManager manager;
	private CargoManager cargo;
	private int rotationID1 = 255, rotationID2 = 255;

	// Public values
	public GameObject indicatorRect, hookPrefab, hookAnchor, brokenCopter, explosion, splash;
	public bool isHookDead = false, isHookDown = false, isKill = false, isSplash = false;
	public float 	maxTilt = 75f, 
					tiltSpeed = 50f, 
					returnSpeed = 5f,
					holdTime = 0.25f,
					rotationSensitivity = 0.5f,
					powerSensitivity = 0.5f,
					powerMultiplier = 100f,
					minPower = 0f,
					maxPower = 120f,
					initialPower = 75f,
					hookDistance = 1.5f,
					reelSpeed = 0.05f;

	public void resetPower() {
		currentPower = initialPower;
	}
	
	// Use this for initialization
	void Start () {
		manager = (LevelManager) GameObject.Find("LevelManagerO").GetComponent(typeof(LevelManager));
		cargo = GetComponent<CargoManager>();
		copterBody = gameObject.GetComponent<Rigidbody2D>();
		powerIndRect = indicatorRect.GetComponent<RectTransform> ();
		hookJoint = GetComponent<DistanceJoint2D> ();
		hookJoint.anchor = hookAnchor.transform.localPosition;
		copterScale = gameObject.transform.localScale.x;
		tempHoldTime = holdTime;
		powerIndPosition = new Vector2(0f, ((Screen.height*manager.uiLiftPowerDeadZone)*(maxPower-(2*currentPower)+minPower)+(currentPower-minPower)*Screen.height)/(maxPower-minPower));
		resetPower();
	}

	//Update is called before void Update();
	void FixedUpdate () {
		copterAngle = gameObject.transform.eulerAngles.z;
	}

	// Update is called once per frame
	void Update () {

		// START INPUT ------------------------------------------------------------------------------------------------------------------------------------------

		// Control system
		if (Input.touchCount > 0) {
			foreach (Touch touch in Input.touches){

				//Copter press and Joystick initialization since both cannot happen during the same frame
				if(touch.phase == TouchPhase.Began && 
				   gameObject.GetComponent<Collider2D>() == Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(touch.position)) && !isHookDead){
					if (isHookDown){
						isHookDown = false;
					} else {
						isHookDown = true;
					}
				}

				//Joystick initialization
				if (touch.phase == TouchPhase.Began) {
					currentAngle = copterAngle;
					if (rotationID1 == 255) {
						rotationID1 = touch.fingerId;
					} else if (rotationID2 == 255) {
						rotationID2 = touch.fingerId;
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
					if (currentPower <= maxPower && currentPower >= minPower) {
						currentPower += (touch.deltaPosition.y/Screen.height)*((maxPower-minPower)*powerSensitivity);
						//Debug.Log(((touch.deltaPosition.y/Screen.height)*((maxPower-minPower)*powerSensitivity)) + " = (" + touch.deltaPosition.y + " / " + Screen.height + ") * (" + (maxPower - minPower) + " * " + powerSensitivity + ")");
					}
				}

				//Control destruction
				if (touch.phase == TouchPhase.Ended) {
					if (touch.fingerId == rotationID1) {
						rotationID1 = 255;
					} else if (touch.fingerId == rotationID2) {
						rotationID2 = 255;
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

		// for limits
		if (currentPower < minPower) currentPower = minPower;
		if (currentPower > maxPower) currentPower = maxPower;
		
		if (minPower > maxPower) minPower = maxPower;
		if (maxPower < minPower) maxPower = minPower;

		//Copter direction mangement
		if (currentAngle > 180f && rotationID1 != 255) gameObject.transform.localScale = new Vector3(copterScale, gameObject.transform.localScale.y); 
		else if (currentAngle < 180f && rotationID1 != 255) gameObject.transform.localScale = new Vector3(-copterScale, gameObject.transform.localScale.y);

		// END INPUT ------------------------------------------------------------------------------------------------------------------------------------------

		copterBody.AddForce (gameObject.transform.up * (currentPower*powerMultiplier) * Time.deltaTime);

		if (isHookDown && hook == null && !isHookDead) {
			once = true;
			hook = Instantiate (hookPrefab, gameObject.transform.position + new Vector3 (0f, -0.3f), Quaternion.identity) as GameObject;
			hookJoint.enabled = true;
			hookJoint.distance = hookDistance;
			hookJoint.connectedBody = hook.GetComponent<Rigidbody2D>();
		} else if (!isHookDown && once && Vector2.Distance (hook.transform.position, hookAnchor.transform.position) < 0.1 && !isHookDead) {
			cargo.cargoHookedCrates (hook);
			if (cargo.getCargoCrates() >= manager.cargoSize && hook.transform.childCount > 0){
				isHookDown = true;
				Debug.Log("Cargo full");
			} else if (hook.transform.childCount == 0){
				Destroy (hook);
				once = false;
			}
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
			manager.levelFailed(2);
		}

		powerIndPosition = new Vector2(0f, ((Screen.height*manager.uiLiftPowerDeadZone)*(maxPower-(2*currentPower)+minPower)+(currentPower-minPower)*Screen.height)/(maxPower-minPower));
		powerIndRect.anchoredPosition = new Vector2(0, powerIndPosition.y);

		if (isKill) kill();
		if (isSplash) splashKill();
	}

	private void killHook () {
		hookJoint.enabled = false;
		hook.GetComponent<DestroyHookOverTime>().doom();
		hook = null;
		isHookDead = true;
	}

	private void kill() {
		if (hook != null)
			killHook ();
		GameObject newCopter = (GameObject) Instantiate(brokenCopter, transform.position, Quaternion.identity);
		Instantiate(explosion, transform.position, Quaternion.identity);
		Rigidbody2D[] parts = newCopter.GetComponentsInChildren<Rigidbody2D>();
		foreach(Rigidbody2D part in parts){
			part.velocity += gameObject.GetComponent<Rigidbody2D>().velocity;
			part.angularVelocity += gameObject.GetComponent<Rigidbody2D>().angularVelocity;
		}
		newCopter.GetComponent<ExplodeParts>().enabled = true;
		Destroy (gameObject);
	}

	private void splashKill() {
		if (hook != null)
			killHook ();
		GameObject newCopter = (GameObject) Instantiate(brokenCopter, transform.position, Quaternion.identity);
		Vector3 splashPos = new Vector3(transform.position.x, manager.getWaterLevel()+0.5f);
		Instantiate(splash, splashPos, Quaternion.identity);
		Rigidbody2D[] parts = newCopter.GetComponentsInChildren<Rigidbody2D>();
		foreach(Rigidbody2D part in parts){
			part.velocity += gameObject.GetComponent<Rigidbody2D>().velocity;
			part.angularVelocity += gameObject.GetComponent<Rigidbody2D>().angularVelocity;
		}
		newCopter.GetComponent<ExplodeParts>().enabled = false;
		Destroy (gameObject);
	}
}
