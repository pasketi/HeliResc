using UnityEngine;
using System.Collections;

public class HookLineManager : MonoBehaviour {

	private LineRenderer line;
	public GameObject copter;

	// Use this for initialization
	void Start () {
		line = gameObject.GetComponent<LineRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		line.SetPosition (0, gameObject.transform.position);
		line.SetPosition (1, copter.transform.position + new Vector3(0f, -0.3f, 0f));
	}
}
