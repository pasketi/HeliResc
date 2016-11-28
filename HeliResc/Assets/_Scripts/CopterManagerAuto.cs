using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CopterManagerAuto : MonoBehaviour {

	private Rigidbody2D copterBody;
	private DistanceJoint2D hookJoint;
	private bool isHookDown = true;

	public GameObject hookPrefab, hookAnchor;
	private GameObject hook;
	public float 	X = 25f, 
					tiltSpeed = 100f, 
					returnSpeed = 5f, 
					power = 20f,
					flyingAltitude = 4f, 
					maxX = 15f,
					hookDistance = 1.5f;

// Use this for initialization
	void Start () {
		copterBody = gameObject.GetComponent<Rigidbody2D>();
		hookJoint = GetComponent<DistanceJoint2D> ();
		hookJoint.anchor = hookAnchor.transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {

		// Helicopter turn to X degrees
		if (gameObject.transform.eulerAngles.z != X) { // Turn to currentAngle
			if (X < 180f) {
				if (gameObject.transform.eulerAngles.z > 180f) {
					// Rotate CCW
					gameObject.transform.Rotate(new Vector3(0f, 0f, tiltSpeed * Time.deltaTime));
				} else if (gameObject.transform.eulerAngles.z < 180f) {
					if (gameObject.transform.eulerAngles.z < X){
						// Rotate CCW
						gameObject.transform.Rotate (new Vector3 (0f, 0f, tiltSpeed * Time.deltaTime));
					} else if (gameObject.transform.eulerAngles.z > X) {
						// Rotate CW
						gameObject.transform.Rotate(new Vector3(0f, 0f, -tiltSpeed * Time.deltaTime));
					}
				}
			} else if (X > 180f) {
				if (gameObject.transform.eulerAngles.z < 180f) {
					// Rotate CW
					gameObject.transform.Rotate (new Vector3 (0f, 0f, -tiltSpeed * Time.deltaTime));
				} else if (gameObject.transform.eulerAngles.z > 180f) {
					if (gameObject.transform.eulerAngles.z < X) {
						// Rotate CCW
						gameObject.transform.Rotate (new Vector3 (0f, 0f, tiltSpeed * Time.deltaTime));
					} else if (gameObject.transform.eulerAngles.z > X) {
						// Rotate CW
						gameObject.transform.Rotate(new Vector3(0f, 0f, -tiltSpeed * Time.deltaTime));
					}
				}
			}
		}
		
		// Automatic 
		if (gameObject.transform.position.y < flyingAltitude) {
			copterBody.AddForce (gameObject.transform.up * (power*100) * Time.deltaTime);
		} else if (gameObject.transform.position.y > flyingAltitude) {
			copterBody.AddForce ((gameObject.transform.up - new Vector3(0f, gameObject.transform.up.y, 0f)) * (power*100) * Time.deltaTime);
		}

		if (isHookDown && hook == null) {
			hook = Instantiate (hookPrefab, gameObject.transform.position + new Vector3 (0f, -0.3f), Quaternion.identity) as GameObject;
			hookJoint.enabled = true;
			hookJoint.distance = hookDistance;
			hookJoint.connectedBody = hook.GetComponent<Rigidbody2D>();
		}

		if (gameObject.transform.position.x >= maxX) {
			gameObject.transform.position = new Vector3(-(gameObject.transform.position.x), gameObject.transform.position.y + ((Random.value-0.5f)*3));
			hook.transform.position = new Vector3(-(hook.transform.position.x), hook.transform.position.y);
		}
	}
}
