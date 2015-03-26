using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelMapButton : MonoBehaviour {

	public int levelIndex;

	public bool levelLocked;

	public Image[] stars;

	public Sprite starUnlocked;
	public Sprite starLocked;

	[Range(0, 3)]
	public int starsAwarded;

	public Text buttonText;

	// Use this for initialization
	void Start () {
		buttonText.text = levelIndex.ToString ();

		if(!levelLocked) {
			for(int i = 0; i < stars.Length; i++) {
				stars[i].sprite = i < starsAwarded ? starUnlocked : starLocked;
			}
		}
	}

	public void StartLevel() {
		Application.LoadLevel("TestScene");
	}
}
