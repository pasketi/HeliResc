using UnityEngine;
using System.Collections;

public class BuoyRockingScript : MonoBehaviour {

	public float intensity = 25f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		gameObject.transform.eulerAngles = new Vector3(0f, 0f, Mathf.Sin(Time.time)*intensity);
	}
}
