using UnityEngine;
using System.Collections;

public static class SaveLoad {

	public static void SaveLevelInfo(LevelInfo info) {
		/*

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

		public void SetLocked(bool locked) {
			PlayerPrefsExt.SetBool(saveName + "Locked", locked);
			levelLocked = locked;
		}

		public void SetStars(int stars) {
			PlayerPrefs.SetInt(saveName + "Stars", stars);
			starsAwarded = stars;
		}

		 */
	}

	public static LevelInfo LoadLevelInfo(string name) {
		LevelInfo info = new LevelInfo ();

		return info;
	}
	
}

public class LevelInfo {
	public string name;
	public bool star1;
	public bool star2;
	public bool star3;
	public bool locked;

	public LevelInfo() {
	
	}
}
