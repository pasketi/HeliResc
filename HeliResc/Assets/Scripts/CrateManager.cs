using UnityEngine;
using System.Collections;

public class CrateManager : MonoBehaviour {

	private LevelManager manager;
	private CopterManagerTouch copterScript;
	private CargoManager cargoScript;
	private float floatyValue; //buoancy?
	private GameObject crate;
	private SpriteRenderer spriteRenderer;
	private DistanceJoint2D joint;
	private HingeJoint2D hinge;
	public float crateMass = 5f;
	public float inWaterModifier = 0.2f;

	public Sprite Dropped, Hooked;
	public bool inWater = false;



	// Use this for initialization
	void Start () {
		manager = (LevelManager) GameObject.Find("LevelManagerO").GetComponent(typeof(LevelManager));
		copterScript = (CopterManagerTouch) GameObject.Find ("Copter").GetComponent(typeof(CopterManagerTouch));
		cargoScript = (CargoManager) GameObject.Find ("Copter").GetComponent(typeof(CargoManager));
		crate = gameObject.transform.parent.gameObject;
		spriteRenderer = crate.GetComponent<SpriteRenderer>();
		floatyValue = gameObject.transform.parent.rigidbody2D.mass * 30f;
		hinge = GetComponent<HingeJoint2D>();
	}
	
	// Update is called once per frame
	void Update () {
		if ((copterScript != null && crate.layer == 11 && copterScript.isHookDead == true) || copterScript == null) {
			Destroy(joint);
			cargoScript.changeCargoMass (-crateMass);
			gameObject.collider2D.enabled = true;
			crate.layer = 10;
			gameObject.transform.parent.parent = null;
		}
		if (crate.transform.position.y < manager.getWaterLevel ()) {
			crate.rigidbody2D.AddForce (Vector3.up * floatyValue);
			inWater = true;
		} else {
			inWater = false;
		}
		if (crate.layer == 11) {
			spriteRenderer.sprite = Hooked;
		} else {
			spriteRenderer.sprite = Dropped;
		}
	}

	void OnCollisionEnter2D(Collision2D collision){
		if (copterScript != null && collision.collider.gameObject.CompareTag ("Hook") && copterScript.isHookDead == false) {
			gameObject.transform.parent.parent = collision.collider.gameObject.transform;
			gameObject.AddComponent ("DistanceJoint2D");

			joint = GetComponent<DistanceJoint2D> ();


			joint.connectedBody = collision.collider.gameObject.rigidbody2D;
			joint.connectedAnchor = new Vector2 (0f, -0.3f);
			joint.distance = 0f;
			joint.maxDistanceOnly = false;
			hinge.useLimits = false;

			gameObject.collider2D.enabled = false;
			crate.layer = 11; //liftedCrate
			cargoScript.changeCargoMass (crateMass);
		}
	}
}
