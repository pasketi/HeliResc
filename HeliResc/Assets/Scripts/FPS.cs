using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FPS : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float FPSt = 1f/Time.deltaTime;
		gameObject.GetComponent<Text>().text = ((int)FPSt).ToString();
	}
}
