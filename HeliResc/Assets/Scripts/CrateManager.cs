using UnityEngine;
using System.Collections;

public class CrateManager : MonoBehaviour {

	private LevelManager manager;
	private float floatyValue = 0.005f; //buoancy?
	private GameObject crate;
	private SpriteRenderer spriteRenderer;

	public Sprite Dropped, Hooked;

	// Use this for initialization
	void Start () {
		manager = (LevelManager) GameObject.Find("LevelManagerO").GetComponent(typeof(LevelManager));
		crate = gameObject.transform.parent.gameObject;
		spriteRenderer = crate.GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		if (crate.transform.position.y < manager.getWaterLevel ()) {
			crate.rigidbody2D.AddForce (Vector3.up * floatyValue);
		}
		if (gameObject.transform.parent.gameObject.layer == 11) {
			spriteRenderer.sprite = Hooked;
		} else {
			spriteRenderer.sprite = Dropped;
		}
	}

	void OnCollisionEnter2D(Collision2D collision){
		if (collision.collider.gameObject.CompareTag("Hook")) {
			gameObject.transform.parent.parent = collision.collider.gameObject.transform;
			gameObject.AddComponent("DistanceJoint2D");

			DistanceJoint2D joint = GetComponent<DistanceJoint2D>();
			HingeJoint2D hinge = GetComponent<HingeJoint2D>();

			joint.connectedBody = collision.collider.gameObject.rigidbody2D;
			joint.connectedAnchor = new Vector2(0f, -0.3f);
			joint.distance = 0f;
			joint.maxDistanceOnly = false;
			hinge.useLimits = false;

			gameObject.collider2D.enabled = false;
			crate.layer = 11; //liftedCrate
		}
	}
}
