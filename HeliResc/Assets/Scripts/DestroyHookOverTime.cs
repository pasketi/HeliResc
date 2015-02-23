using UnityEngine;
using System.Collections;

public class DestroyHookOverTime : MonoBehaviour {

	private float lived = 0f;
	public bool doomed = false;
	public float timeToLive = 5f;

	public void doom(){
		doomed = true;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (doomed) {
			lived += Time.deltaTime;
			if (lived > timeToLive)
				Destroy (gameObject);
		}
	}
}
