using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LevelMapHandler : MonoBehaviour {

	public Text starsText;				//UI text component to show the amount of stars the player has
	public Text rubiesText;				//UI text to show the amount of rubies the player has

	// Use this for initialization
	void Start () {

		int stars = PlayerPrefs.GetInt (SaveStrings.sPlayerStars, 0);
		int rubies = PlayerPrefs.GetInt (SaveStrings.sPlayerRubies, 0);
		int maxStars = LevelHandler.LevelCount * 3;
		int maxRubies = LevelHandler.LevelCount;

		starsText.text = stars + "/" + maxStars;
		rubiesText.text = rubies + "/" + maxRubies;
	}

	public void PressMainMenu(int menu) {
		GameManager manager = GameObject.FindObjectOfType<GameManager> ();
		manager.loadMainMenu (false, null, menu);
	}
}
