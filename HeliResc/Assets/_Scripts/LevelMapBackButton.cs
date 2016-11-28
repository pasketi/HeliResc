using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelMapBackButton : MonoBehaviour {

	private GameManager g;

	public Button backButton;

	// Use this for initialization
	void Start () {
		g = GameObject.Find ("GameManager").GetComponent<GameManager>();

		backButton = GetComponent<Button>();

		backButton.onClick.AddListener(() => g.loadMainMenu(false, null));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
