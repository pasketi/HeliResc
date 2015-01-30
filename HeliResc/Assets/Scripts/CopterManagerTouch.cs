using UnityEngine;
using System.Collections;

public class CopterManagerTouch : MonoBehaviour {

	private SpriteRenderer spriteRenderer;
	private Rigidbody2D copterBody;
	private bool running = true, lastMovementRight = true;
	private Vector2 lastVelocity;
	private Touch[] touches;

	public Sprite copterSprite1, copterSprite2;
	public GameObject indicator;
	private LineRenderer altitudeInd;
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
	
	// Use this for initialization
	void Start () {
		copterBody = gameObject.GetComponent<Rigidbody2D>();
		spriteRenderer = GetComponentInChildren<SpriteRenderer>(); 
		Debug.Log(spriteRenderer.gameObject.name);
		Debug.Log (360f - maxTilt);
		altitudeInd = indicator.GetComponent<LineRenderer> ();
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

		//Left rotate
		if (Input.touchCount > 1){}
		else if (Input.touchCount > 0 && Input.GetTouch(0).position.x < Screen.width/2){
			//What happens the first frame
			// FIX FOR TOUCH
			if (lastMovementRight != false){
				gameObject.transform.localScale = new Vector3(-gameObject.transform.localScale.x, gameObject.transform.localScale.y);
				lastMovementRight = false;
			}

			//What happens every frame
			if (gameObject.transform.eulerAngles.z >= maxTilt 
			    && gameObject.transform.eulerAngles.z <= 180f) {} else {
				gameObject.transform.Rotate(new Vector3(0f, 0f, tiltSpeed*Time.deltaTime));
			}
		}

		//Right rotate
		if (Input.touchCount > 1){}
		else if (Input.touchCount > 0 && Input.GetTouch(0).position.x > Screen.width/2) {
			//What happens the first frame
			// FIX FOR TOUCH
			if (lastMovementRight != true) {
				gameObject.transform.localScale = new Vector3(-gameObject.transform.localScale.x, gameObject.transform.localScale.y);
				lastMovementRight = true;
			}

			//What happens every frame
			if (gameObject.transform.eulerAngles.z <= 360f-maxTilt 
			    && gameObject.transform.eulerAngles.z >= 180f) {} else {
				gameObject.transform.Rotate (new Vector3 (0f, 0f, -tiltSpeed * Time.deltaTime));
			}
		}

		// FIX FOR TOUCH
		/*if (Input.GetKeyDown (KeyCode.UpArrow)) {
			if (flyingAltitude < 6.5f)
				flyingAltitude += 1f;
			else flyingAltitude = 6.5f;
		}

		if (Input.GetKeyDown (KeyCode.DownArrow)) {
			if (flyingAltitude > 0.5f)
				flyingAltitude -= 1f;
			else flyingAltitude = 0.5f;
		}*/

		// Helicopter return to 0 degrees
		if (gameObject.transform.eulerAngles.z != 0f && Input.touchCount < 1) {
			if (gameObject.transform.eulerAngles.z > 180f) {
				gameObject.transform.Rotate (new Vector3 (0f, 0f, returnSpeed*Time.deltaTime*(360f-gameObject.transform.eulerAngles.z)));
			} else if (gameObject.transform.eulerAngles.z < 180f) {
				gameObject.transform.Rotate (new Vector3 (0f, 0f, -(returnSpeed*Time.deltaTime)*gameObject.transform.eulerAngles.z));
			}
		}

		//Automatic 
		if (gameObject.transform.position.y < flyingAltitude && running) {
			/*if (copterBody.velocity.y < 0f && lastVelocity.y > copterBody.velocity.y && 
			    power < maxPower && 
			    copterBody.velocity.y < maxVelocity && 
			    power < cruisePower) {
				power += acceleration;
			} else if (gameObject.transform.position.y >= flyingAltitude && power > 0f) {
				power = cruisePower;
			}*/
			copterBody.AddForce (gameObject.transform.up * power);
		} else if (gameObject.transform.position.y > flyingAltitude && running) {
			copterBody.AddForce ((gameObject.transform.up - new Vector3(0f, gameObject.transform.up.y, 0f)) * power);
		}

		if (running) {
			power = cruisePower;
		} else {
			power = 0f;
		}

		altitudeInd.SetPosition (0, new Vector3 (9f, flyingAltitude, 0f));
		altitudeInd.SetPosition (1, new Vector3 (-9f, flyingAltitude, 0f));

		//Debug.Log (power);
		//lastVelocity = copterBody.velocity;
	}
}
