using UnityEngine;
using System.Collections;

public class HookLineManager : MonoBehaviour {

	private LineRenderer line;
	private bool isHookDown = true;
	public GameObject copter, anchor;

	// Use this for initialization
	void Start () {
		line = gameObject.GetComponent<LineRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {

		line.SetPosition (0, gameObject.transform.position);
		line.SetPosition (1, anchor.transform.position);
	}

	void OnCollisionEnter2D(Collision2D collision){
		if (collision.collider.gameObject.CompareTag("SaveableObject")) {
			collision.collider.gameObject.transform.parent.parent = gameObject.transform;
			gameObject.AddComponent("DistanceJoint2D");
			DistanceJoint2D joint = GetComponent<DistanceJoint2D>();
			joint.connectedBody = collision.collider.gameObject.rigidbody2D;
			joint.distance = 0f;
			joint.maxDistanceOnly = false;
			gameObject.collider2D.enabled = false;
		}
	}
}
