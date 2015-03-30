using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelMapButton : MonoBehaviour {

	public int levelIndex;

	public bool levelLocked;

	public string levelName;

	public Image[] stars;

	public Sprite starUnlocked;
	public Sprite starLocked;

	private string saveName;

	[Range(0, 3)]
	public int starsAwarded;

	public Text buttonText;

	// Use this for initialization
	void Start () {
		saveName = "Level" + levelIndex.ToString();

		buttonText.text = levelIndex.ToString ();

		if(PlayerPrefs.HasKey(saveName + "Locked"))
			levelLocked = PlayerPrefsExt.GetBool(saveName + "Locked");
		else {
			PlayerPrefsExt.SetBool(saveName + "Locked", levelIndex > 1);
		}

		if(PlayerPrefs.HasKey(saveName + "Stars")) {
			starsAwarded = PlayerPrefs.GetInt(saveName + "Stars");
		}
		else
			SetStars(0);

		for(int i = 0; i < stars.Length; i++) {
			if(!levelLocked) {
				stars[i].sprite = i < starsAwarded ? starUnlocked : starLocked;
			}
			else {
				stars[i].sprite = starLocked;
			}
		}
	}

	public void SetLocked(bool locked) {
		PlayerPrefsExt.SetBool(saveName + "Locked", locked);
		levelLocked = locked;
	}

	public void SetStars(int stars) {
		PlayerPrefs.SetInt(saveName + "Stars", stars);
		starsAwarded = stars;
	}

	public void StartLevel() {
		if(!levelLocked) 
			Application.LoadLevel(saveName);
	}
}
