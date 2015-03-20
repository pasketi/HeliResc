using UnityEngine;
using System.Collections;

public class SplashManager : MonoBehaviour {

	public float splashTime = 3f, tempTime = 0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		tempTime += Time.deltaTime;
		if (tempTime >= splashTime) {
			Application.LoadLevel("MainMenu");
		}
	}
}
