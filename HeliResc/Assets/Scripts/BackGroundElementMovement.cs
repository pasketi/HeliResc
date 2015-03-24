using UnityEngine;
using System.Collections;

public class BackGroundElementMovement : MonoBehaviour {

	private Vector3 lastCamPos, deltaPos;
	public float speedInPercentX = 0.5f, speedInPercentY = 0.5f;

	// Use this for initialization
	void Start () {
		lastCamPos = Camera.main.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		deltaPos = new Vector3(Camera.main.transform.position.x - lastCamPos.x, Camera.main.transform.position.y - lastCamPos.y);
		transform.position = new Vector3 (transform.position.x + (deltaPos.x*speedInPercentX), transform.position.y + (deltaPos.y*speedInPercentY));
		lastCamPos = Camera.main.transform.position;
	}
}
