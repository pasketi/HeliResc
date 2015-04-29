using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SetReset : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<Button> ().onClick.AddListener(() => GameObject.Find ("GameManager").GetComponent<GameManager>().resetData());
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
