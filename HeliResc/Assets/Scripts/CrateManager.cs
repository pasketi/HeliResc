using UnityEngine;
using System.Collections;

public class CrateManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D collision){
		if (collision.collider.gameObject.CompareTag("Hook")) {
			gameObject.transform.parent.parent = collision.collider.gameObject.transform;
			gameObject.AddComponent("DistanceJoint2D");
			DistanceJoint2D joint = GetComponent<DistanceJoint2D>();
			HingeJoint2D hinge = GetComponent<HingeJoint2D>();
			joint.connectedBody = collision.collider.gameObject.rigidbody2D;
			joint.distance = 0f;
			joint.maxDistanceOnly = false;
			hinge.useLimits = false;
			gameObject.collider2D.enabled = false;
			gameObject.transform.parent.gameObject.layer = 11; //liftedCrate
		}
	}
}
