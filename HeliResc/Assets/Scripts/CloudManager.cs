using UnityEngine;
using System.Collections;

public class CloudManager : MonoBehaviour {

	private Transform copter;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (GameObject.Find ("Copter") != null && copter == null)
			copter = GameObject.Find ("Copter").transform;
		else if (copter != null) transform.position = new Vector3 (copter.position.x, transform.position.y);
	}
}
