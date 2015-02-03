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
}
