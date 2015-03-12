using UnityEngine;
using System.Collections;

public class CrateManager : MonoBehaviour {

	private LevelManager manager;
	private CopterManagerTouch copterScript;
	private CargoManager cargoScript;
	private float floatyValue; //buoancy?
	private bool lastInWater = false;
	private GameObject crate;
	private SpriteRenderer spriteRenderer, bgRenderer;
	private DistanceJoint2D joint;
	private HingeJoint2D hinge;
	public float crateMass = 5f;
	public float inWaterModifier = 0.2f;

	public Sprite Dropped, Hooked, DroppedBG, HookedBG;
	public bool inWater = false, inCargo = false, inMenu = false;



	// Use this for initialization
	void Start () {
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
		floatyValue = gameObject.transform.parent.GetComponent<Rigidbody2D> ().mass * 30f;
		hinge = GetComponent<HingeJoint2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!inMenu) {
			if ((!inCargo && copterScript != null && crate.layer == 11 && copterScript.isHookDead == true) || copterScript == null) {
				Destroy (joint);
				cargoScript.changeHookMass (-crateMass);
				gameObject.GetComponent<Collider2D> ().enabled = true;
				crate.layer = 10;
				gameObject.transform.parent.parent = null;
			}

			if (crate != null && (!inCargo && crate.transform.position.y < manager.getWaterLevel ())) {
				if (manager.getPaused () == false)
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
				spriteRenderer.sprite = Dropped;
				if (bgRenderer != null)
					bgRenderer.sprite = DroppedBG;
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
