using UnityEngine;
using System.Collections;

public class DroppingScript : MonoBehaviour {

	private Rigidbody2D _rb;
	private Transform _transform;
	private BoxCollider2D _box;

	void Start() {
		_rb = GetComponent<Rigidbody2D> ();
		_transform = transform;
		_box = GetComponent<BoxCollider2D> ();
	}

	void FixedUpdate () {
		if (_box.enabled == false)
			_transform.up = Vector3.Lerp (_transform.up, -_rb.velocity.normalized, 0.1f); 	// = -_rb.velocity.normalized;
	}
	void OnTriggerExit2D(Collider2D other) {
		DisableCollider ();
	}

	void DisableCollider() {
		_box.enabled = false;
	}
}
