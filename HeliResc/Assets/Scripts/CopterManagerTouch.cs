using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CopterManagerTouch : MonoBehaviour {

	private SpriteRenderer spriteRenderer;
	private Rigidbody2D copterBody;
	private DistanceJoint2D hookJoint;
	private bool running = true, lastMovementRight = true, isHookDown = true, once = false;
	private Vector2 lastVelocity;
	private int lastTouchCount = 0;
	private LevelManager manager;

	public GameObject indicatorRect, hookPrefab, hookAnchor;
	private GameObject hook;
	private RectTransform altitudeIndRect;
	public float 	maxTilt = 75f, 
					tiltSpeed = 100f, 
					returnSpeed = 5f, 
					power = 20f,
					flyingAltitude = 4f, 
					maxVelocity = 3f;

	public void setFlyingAltitude(float altitude){
		flyingAltitude = altitude;
	}

	// Use this for initialization
	void Start () {
		manager = (LevelManager) GameObject.Find("LevelManagerO").GetComponent(typeof(LevelManager));
		copterBody = gameObject.GetComponent<Rigidbody2D>();
		spriteRenderer = GetComponentInChildren<SpriteRenderer>(); 
		Debug.Log(spriteRenderer.gameObject.name);
		Debug.Log (360f - maxTilt);
		altitudeIndRect = indicatorRect.GetComponent<RectTransform> ();
		hookJoint = GetComponent<DistanceJoint2D> ();
		hookJoint.anchor = hookAnchor.transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {

		// START INPUT ------------------------------------------------------------------------------------------------------------------------------------------

		// Copter press
		int i = 0;
		while (Input.touchCount > i){
			if (Input.GetTouch(i).phase == TouchPhase.Began && 
			    gameObject.collider2D == Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position))){
				if (isHookDown){
					isHookDown = false;
				} else {
					isHookDown = true;
				}
			}
			i++;
		}
		i = 0;

		// Left rotate
		if (Input.touchCount > 1){}
		else if (Input.touchCount > 0 && Input.GetTouch(0).position.x < Screen.width/2 && Input.GetTouch(0).position.x > 150){
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
		else if (Input.touchCount > 0 && Input.GetTouch(0).position.x > Screen.width/2 && Input.GetTouch(0).position.x < Screen.width-150) {
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

		// Helicopter return to 0 degrees
		if (gameObject.transform.eulerAngles.z != 0f && Input.touchCount < 1) {
			if (gameObject.transform.eulerAngles.z > 180f) {
				gameObject.transform.Rotate (new Vector3 (0f, 0f, returnSpeed*Time.deltaTime*(360f-gameObject.transform.eulerAngles.z)));
			} else if (gameObject.transform.eulerAngles.z < 180f) {
				gameObject.transform.Rotate (new Vector3 (0f, 0f, -(returnSpeed*Time.deltaTime)*gameObject.transform.eulerAngles.z));
			}
		}

		// New input management maybe?
		if (Input.touchCount == 0) {} 
		else if (Input.touchCount == 1) {

			// Altitude management
			if (Input.GetTouch(0).position.x < 100 || Input.GetTouch(0).position.x > Screen.width -150) {
				float tempY = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position).y;
				if (tempY <= 7.5f && tempY >= 0f){
					setFlyingAltitude(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position).y);
				}
			}
		} else if (Input.touchCount == 2) {

			// Altitude management
			if (Input.GetTouch(1).position.x < 100 || Input.GetTouch(1).position.x > Screen.width -150) {
				float tempY = Camera.main.ScreenToWorldPoint(Input.GetTouch(1).position).y;
				if (tempY <= 7.5f && tempY >= 0f){
					setFlyingAltitude(Camera.main.ScreenToWorldPoint(Input.GetTouch(1).position).y);
				}
			}
		}

		// END INPUT ------------------------------------------------------------------------------------------------------------------------------------------

		// Automatic 
		if (gameObject.transform.position.y < flyingAltitude && running) {
			copterBody.AddForce (gameObject.transform.up * (power*100) * Time.deltaTime);
		} else if (gameObject.transform.position.y > flyingAltitude && running) {
			copterBody.AddForce ((gameObject.transform.up - new Vector3(0f, gameObject.transform.up.y, 0f)) * (power*100) * Time.deltaTime);
		}

		if (isHookDown && hook == null) {
			once = true;
			hook = Instantiate (hookPrefab, gameObject.transform.position + new Vector3 (0f, -0.3f), Quaternion.identity) as GameObject;
			hookJoint.connectedBody = hook.rigidbody2D;
		} else if (!isHookDown && once) {
			manager.cargoHookedCrates(hook);
			Destroy(hook);
			once = false;
		}

		altitudeIndRect.anchoredPosition = new Vector2(0, Camera.main.WorldToScreenPoint(new Vector3(0f, flyingAltitude)).y);

		lastTouchCount = Input.touchCount;
	}
}
