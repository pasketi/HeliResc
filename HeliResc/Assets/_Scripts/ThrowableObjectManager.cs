using UnityEngine;
using System.Collections;

public class ThrowableObjectManager : MonoBehaviour {

	private LevelManager manager;
	private float floatyValue, lifeTime = 15f;

	// Use this for initialization
	void Start () {
		manager = GameObject.Find ("LevelManagerO").GetComponent<LevelManager> ();
		floatyValue = GetComponent<Rigidbody2D> ().mass * 25f;
	}
	
	// Update is called once per frame
	void Update () {
		lifeTime -= Time.deltaTime;
		if (manager.getPaused () == false && transform.position.y < manager.getWaterLevel ())
			GetComponent<Rigidbody2D>().AddForce (Vector3.up * floatyValue);
		if (lifeTime <= 0f)
			Destroy (gameObject);
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "ActionableObject" && other.gameObject.layer != 11) {
			other.gameObject.transform.parent.gameObject.layer = 11;
			Destroy(gameObject);
		}
	}
}
