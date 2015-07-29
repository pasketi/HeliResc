using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GetButtonMethods : MonoBehaviour {


	public Button btnResetAll;
	public Button btnPlay;

	private GameManager g;
	// Use this for initialization
	void Start () {
		g = GameObject.Find ("GameManager").GetComponent<GameManager>();

		
		btnResetAll.onClick.AddListener (() => g.resetData ());
	}
}
