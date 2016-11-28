using UnityEngine;
using System.Collections;

public class PlayerPrefsExt {

	public static void SetBool(string key, bool value) {
		PlayerPrefs.SetInt(key, value ? 1 : 0);
	}

	public static bool GetBool(string key) {
		if (PlayerPrefs.HasKey (key))
			return PlayerPrefs.GetInt (key) > 0;
		else
			return false;
	}

}
