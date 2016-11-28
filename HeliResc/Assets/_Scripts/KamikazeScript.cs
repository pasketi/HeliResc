using UnityEngine;
using System.Collections;

public class KamikazeScript : MonoBehaviour {

	public float speed = 1f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt (GameObject.Find ("Copter").transform.position);
		transform.position = Vector3.MoveTowards (transform.position, GameObject.Find ("Copter").transform.position, speed * Time.deltaTime);
		transform.Rotate (0, -90, 0);
	}
}
