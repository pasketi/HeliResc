﻿using UnityEngine;
using System.Collections;
using System;

public static class SaveLoad {

	public static void SaveLevelInfo(LevelInfo info) {

        string name = "Level" + info.index.ToString();

        PlayerPrefsExt.SetBool(name + "Locked", info.locked);
        PlayerPrefsExt.SetBool(name + "Star1", info.star1);
        PlayerPrefsExt.SetBool(name + "Star2", info.star2);
        PlayerPrefsExt.SetBool(name + "Star3", info.star3);
	}

	public static LevelInfo LoadLevelInfo(int index) {
        string level = "Level" + index.ToString();
        bool locked = false;
        bool s1 = false;
        bool s2 = false;
        bool s3 = false;
        try {
            locked = PlayerPrefsExt.GetBool(level + "Locked");
            s1 = PlayerPrefsExt.GetBool(level + "Star1");
            s2 = PlayerPrefsExt.GetBool(level + "Star2");
            s3 = PlayerPrefsExt.GetBool(level + "Star3");
        }
        catch (Exception ex) {
            Debug.LogError("No key in player prefs: " + ex.Message);
        }

        LevelInfo info = new LevelInfo (index, s1, s2, s3, locked);
		return info;
	}
	
}

public class LevelInfo {
    public int index;

	public bool star1;
	public bool star2;
	public bool star3;
	public bool locked;


	public LevelInfo(int i, bool s1, bool s2, bool s3, bool l) {
        index = i;

        star1 = s1;
        star2 = s2;
        star3 = s3;

        locked = l;
	}
}
