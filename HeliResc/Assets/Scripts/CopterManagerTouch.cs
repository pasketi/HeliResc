using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CopterManagerTouch : MonoBehaviour {

	private SpriteRenderer spriteRenderer;
	private Rigidbody2D copterBody;
	private bool running = true, lastMovementRight = true, isHookDown = true;
	private Vector2 lastVelocity;
	private int lastTouchCount;

	public Sprite copterSprite1, copterSprite2;
	public GameObject indicatorRect;
	private RectTransform altitudeIndRect;
	public float 	maxTilt = 75f, 
					tiltSpeed = 100f, 
					returnSpeed = 5f, 
					//acceleration = 5f, 
					maxPower = 500f, 
					power = 0f, 
					cruisePower = 200f, 
					flyingAltitude = 4f, 
					maxVelocity = 3f;
	public AnimationCurve powerUp, powerDown;

	float lastTime = 0f, frameTime = 0.125f;

	public void setFlyingAltitude(float altitude){
		flyingAltitude = altitude;
	}

	// Use this for initialization
	void Start () {
		copterBody = gameObject.GetComponent<Rigidbody2D>();
		spriteRenderer = GetComponentInChildren<SpriteRenderer>(); 
		Debug.Log(spriteRenderer.gameObject.name);
		Debug.Log (360f - maxTilt);
		altitudeIndRect = indicatorRect.GetComponent<RectTransform> ();
		//Input.simulateMouseWithTouches == true;
	}
	
	// Update is called once per frame
	void Update () {

		// Fix for touch
		/*
		if (Input.GetKeyDown (KeyCode.LeftControl))
						running = !running;
		*/
		
		//Copter animation handling
		if (lastTime < Time.time - frameTime){
			if (spriteRenderer.sprite.name == copterSprite1.name) spriteRenderer.sprite = copterSprite2;
			else if (spriteRenderer.sprite.name == copterSprite2.name) spriteRenderer.sprite = copterSprite1;
			lastTime = Time.time;
		}

		//Copter Press
		/*if (Input.touchCount > 0)
		{
			//for (int i = 0; i < Input.touchCount; i++) {
				Vector3 wp = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position);
				Vector2 touchPos = new Vector2(wp.x, wp.y);
				if (copter.collider2D == Physics2D.OverlapPoint(touchPos))
				{
					if (isHookDown){
						Debug.Log ("Reel In");
						isHookDown = false;
					} else {
						Debug.Log ("Drop Down");
						isHookDown = true;
					}
				}
			//}
		}*/

		// START INPUT ------------------------------------------------------------------------------------------------------------------------------------------

		// Copter press
		int i = 0;
		while (Input.touchCount > i){
			if (Input.GetTouch(i).phase == TouchPhase.Began && 
			    gameObject.collider2D == Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position))){
				if (isHookDown){
					Debug.Log ("Reel In");
					isHookDown = false;
				} else {
					Debug.Log ("Drop Down");
					isHookDown = true;
				}
			}
			i++;
		}
		i = 0;

		// Left rotate
		if (Input.touchCount > 1){}
		else if (Input.touchCount > 0 && Input.GetTouch(0).position.x < Screen.width/2 && Input.GetTouch(0).position.x > Screen.width*0.1f){
			//  happens the first frame
			// FIX FOR TOUCH
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
		else if (Input.touchCount > 0 && Input.GetTouch(0).position.x > Screen.width/2 && Input.GetTouch(0).position.x < Screen.width*0.9f) {
			// What happens the first frame
			// FIX FOR TOUCH
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

		// Engine power toggle
		if (running) {
			power = cruisePower;
		} else {
			power = 0f;
		}

		// New input management maybe?
		if (Input.touchCount == 0) {} 
		else if (Input.touchCount == 1) {

			// Altitude management
			if (Input.GetTouch(0).position.x < Screen.width * 0.1f || Input.GetTouch(0).position.x > Screen.width * 0.9f) {
				float tempY = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position).y;
				if (tempY <= 7.5f && tempY >= 0f){
					setFlyingAltitude(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position).y);
				}
			}
		} else if (Input.touchCount == 2) {

			// Altitude management
			if (Input.GetTouch(1).position.x < Screen.width * 0.1f || Input.GetTouch(1).position.x > Screen.width * 0.9f) {
				float tempY = Camera.main.ScreenToWorldPoint(Input.GetTouch(1).position).y;
				if (tempY <= 7.5f && tempY >= 0f){
					setFlyingAltitude(Camera.main.ScreenToWorldPoint(Input.GetTouch(1).position).y);
				}
			}
		}

		// END INPUT ------------------------------------------------------------------------------------------------------------------------------------------

		// Automatic 
		if (gameObject.transform.position.y < flyingAltitude && running) {
			copterBody.AddForce (gameObject.transform.up * power);
		} else if (gameObject.transform.position.y > flyingAltitude && running) {
			copterBody.AddForce ((gameObject.transform.up - new Vector3(0f, gameObject.transform.up.y, 0f)) * power);
		}

		altitudeIndRect.anchoredPosition = new Vector2(0, Camera.main.WorldToScreenPoint(new Vector3(0f, flyingAltitude)).y);

		lastTouchCount = Input.touchCount;
	}
}
