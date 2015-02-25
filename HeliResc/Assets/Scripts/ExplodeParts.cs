using UnityEngine;
using System.Collections;

public class ExplodeParts : MonoBehaviour {

	private bool exploded = false;
	private Rigidbody2D[] parts;
	public float intensity = 100f;

	// Use this for initialization
	void Start () {
		parts = GetComponentsInChildren<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		if (!exploded){
			foreach(Rigidbody2D part in parts) {
				part.transform.parent = null;
				part.AddForce(new Vector2((Random.value-0.5f) * intensity, (Random.value-0.5f) * intensity));
				part.AddTorque((Random.value-0.5f) * intensity/10);
			}
			exploded = true;
		}
	}
}
