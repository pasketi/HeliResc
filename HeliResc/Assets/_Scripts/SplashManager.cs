using UnityEngine;
using System.Collections;

public class SplashManager : MonoBehaviour {

    public bool playAsFirstTime = false;

	public float splashTime = 3f, tempTime = 0f;

	// Use this for initialization
	void Start () {
        if (playAsFirstTime)
            PlayerPrefs.DeleteAll();
	}
	
	// Update is called once per frame
	void Update () {
		tempTime += Time.deltaTime;
		if (tempTime >= splashTime) {
			Application.LoadLevel("MainMenu");
		}
	}
}
