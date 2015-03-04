using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class heightWidthDebug : MonoBehaviour {

	private Text debugText;

	// Use this for initialization
	void Start () {
		debugText = GetComponent<Text>();
		debugText.text = ("Height: " + Screen.height + " Width: " + Screen.width);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
