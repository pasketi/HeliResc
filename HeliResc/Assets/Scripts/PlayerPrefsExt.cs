using UnityEngine;
using System.Collections;

public class PlayerPrefsExt {

	public static void SetBool(string key, bool value) {
		PlayerPrefs.SetInt(key, value ? 1 : 0);
	}

	public static bool GetBool(string key) {
		return PlayerPrefs.GetInt(key) > 0;
	}

}
