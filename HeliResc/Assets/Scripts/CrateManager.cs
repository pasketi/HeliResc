using UnityEngine;
using System.Collections;

public class CrateManager : MonoBehaviour {

	private LevelManager manager;
	private CopterManagerTouch copterScript;
	private CargoManager cargoScript;
	private float floatyValue, maxLifeTimeInSeconds, flashTime = 0.25f, tempFlash = 0f; //buoancy?
	private bool 	lastInWater = false,
					stationary = false,
					twoPhases = false,
					fourPhases = false,
					flashWhite = false;
	private GameObject crate;
	private SpriteRenderer spriteRenderer, bgRenderer;
	private DistanceJoint2D joint;
	private HingeJoint2D hinge;
	public float crateMass = 5f, inWaterModifier = 0.2f, lifeTimeInSeconds = 60f;

	public Sprite 	Phase100, 	//MANDATORY, First sprite
					Phase66,	//four phase, max 2/3s left
					Phase33, 	//four phase, max 1/3s left
					Phase0,		//two phase, dead
					Hooked, 	//MANDATORY
					Phase100BG,
					Phase66BG,
					Phase33BG,
					Phase0BG, 
					HookedBG;
	public bool 	inWater = false, 
					inCargo = false, 
					inMenu = false, 
					dying = false, 
					dead = false;



	// Use this for initialization
	void Start () {
		maxLifeTimeInSeconds = lifeTimeInSeconds;
		if (!inMenu) {
			Time.timeScale = 1f;
			manager = GameObject.Find ("LevelManagerO").GetComponent<LevelManager> ();
			copterScript = GameObject.Find ("Copter").GetComponent<CopterManagerTouch> ();
			cargoScript = GameObject.Find ("Copter").GetComponent<CargoManager> ();
		}
		crate = gameObject.transform.parent.gameObject;
		spriteRenderer = crate.GetComponent<SpriteRenderer> ();
		if (crate.transform.FindChild ("BackGround") != null)
			bgRenderer = crate.transform.FindChild ("BackGround").GetComponent<SpriteRenderer> ();

		if (Phase100 != null && Phase66 == null && Phase33== null && Phase0 == null){
			stationary = true;
		} else if (Phase100 != null && Phase66 == null && Phase33 == null && Phase0 != null){
			twoPhases = true;
		} else if (Phase100 != null && Phase66 != null && Phase33 != null && Phase0 != null){
			fourPhases = true;
		}

		floatyValue = gameObject.transform.parent.GetComponent<Rigidbody2D> ().mass * 30f;
		hinge = GetComponent<HingeJoint2D> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (!inMenu) {

			if (dying && crate.layer != 11) {
				lifeTimeInSeconds -= Time.deltaTime;
				if (lifeTimeInSeconds <= 0f) dead = true;
			}

			if (dying && twoPhases) {
				if (lifeTimeInSeconds <= 0f) {
					spriteRenderer.sprite = Phase0;
					if (Phase0BG != null) bgRenderer.sprite = Phase0BG;
				} else {
					spriteRenderer.sprite = Phase100;
					if (Phase100BG != null) bgRenderer.sprite = Phase100BG;
				}
			} else if (dying && fourPhases) {
				if (lifeTimeInSeconds <= (maxLifeTimeInSeconds/3)*2) {
					spriteRenderer.sprite = Phase66;
					if (Phase66BG != null) bgRenderer.sprite = Phase66BG;
				} else if (lifeTimeInSeconds <= (maxLifeTimeInSeconds/3)) {
					spriteRenderer.sprite = Phase33;
					if (Phase33BG != null) bgRenderer.sprite = Phase33BG;
				} else if (lifeTimeInSeconds <= 0f) {
					spriteRenderer.sprite = Phase0;
					if (Phase0BG != null) bgRenderer.sprite = Phase0BG;
				} else {
					spriteRenderer.sprite = Phase100;
					if (Phase100BG != null) bgRenderer.sprite = Phase100BG;
				}
			}

			if (dying) {
				if (lifeTimeInSeconds < 10f) {
					tempFlash += Time.deltaTime;
					if (tempFlash >= flashTime) {
						flashWhite = !flashWhite;
						tempFlash = 0f;
					}
				}

				if (flashWhite) bgRenderer.color = new Color(1f, 1f, 1f);
				else {
				if ((lifeTimeInSeconds / maxLifeTimeInSeconds) > 0.5f) bgRenderer.color = new Color(1f, 1f, (lifeTimeInSeconds - (maxLifeTimeInSeconds / 2)) / (maxLifeTimeInSeconds - (maxLifeTimeInSeconds / 2)));
				else if ((lifeTimeInSeconds / maxLifeTimeInSeconds) <= 0.5f) bgRenderer.color = new Color(1f, lifeTimeInSeconds / (maxLifeTimeInSeconds - (maxLifeTimeInSeconds / 2)), 0f);	
				}
			}

			if ((!inCargo && copterScript != null && crate.layer == 11 && copterScript.isHookDead == true) || copterScript == null) {
				Destroy (joint);
				cargoScript.changeHookMass (-crateMass);
				gameObject.GetComponent<Collider2D> ().enabled = true;
				crate.layer = 10;
				gameObject.transform.parent.parent = null;
			}

			if (crate != null && (!inCargo && crate.transform.position.y < manager.getWaterLevel ())) {
				if (manager.getPaused () == false && !dead)
					crate.GetComponent<Rigidbody2D> ().AddForce (Vector3.up * floatyValue);
				inWater = true;
			} else {
				inWater = false;
			}

			if (crate != null && copterScript != null && crate.layer == 11 && inWater != lastInWater) {
				lastInWater = inWater;
				if (inWater && !inCargo) {
					cargoScript.changeHookMass ((-crateMass) + (crateMass * inWaterModifier));
				} else if (!inWater || inCargo) {
					cargoScript.changeHookMass ((crateMass) + -(crateMass * inWaterModifier));
				}
			}

			if (crate.layer == 11) {
				spriteRenderer.sprite = Hooked;
				if (bgRenderer != null)
					bgRenderer.sprite = HookedBG;
			} else {
				spriteRenderer.sprite = Phase100;
				if (bgRenderer != null)
					bgRenderer.sprite = Phase100BG;
			}
		} else {
			if (crate.transform.position.y < 0f) {
				crate.GetComponent<Rigidbody2D> ().AddForce (Vector3.up * floatyValue);
				inWater = true;
			} else {
				inWater = false;
			}
		}
	}

	void OnCollisionEnter2D(Collision2D collision){
		if (!inMenu) {
			if (copterScript != null && collision.collider.gameObject.CompareTag ("Hook") && copterScript.isHookDead == false) {
				gameObject.transform.parent.parent = collision.collider.gameObject.transform;
				gameObject.AddComponent <DistanceJoint2D> ();

				joint = GetComponent<DistanceJoint2D> ();


				joint.connectedBody = collision.collider.gameObject.GetComponent<Rigidbody2D> ();
				joint.connectedAnchor = new Vector2 (0f, -0.3f);
				joint.distance = 0f;
				joint.maxDistanceOnly = false;
				hinge.useLimits = false;

				gameObject.GetComponent<Collider2D> ().enabled = false;
				crate.layer = 11; //liftedCrate
				cargoScript.changeHookMass (crateMass);
			}
		}
	}
}
