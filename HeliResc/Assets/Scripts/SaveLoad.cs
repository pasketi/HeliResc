using UnityEngine;
using System.Collections;
using System;

public static class SaveLoad {

    private static string strCoins = "CoinsAmount";

    public static void SaveWallet(Wallet wallet) {
        PlayerPrefs.SetInt(strCoins, wallet.Coins);
    }

    public static Wallet LoadWallet() {
        if (PlayerPrefs.HasKey(strCoins)) {
            return new Wallet(PlayerPrefs.GetInt(strCoins));
        }
        else {
            PlayerPrefs.SetInt(strCoins, 0);
            return new Wallet(0);
        }
    }

	public static void SaveLevelInfo(LevelInfo info) {

        string name = "Level" + info.index.ToString();

        PlayerPrefsExt.SetBool(name + "Locked", info.locked);
        PlayerPrefsExt.SetBool(name + "Star1", info.star1);
        PlayerPrefsExt.SetBool(name + "Star2", info.star2);
        PlayerPrefsExt.SetBool(name + "Star3", info.star3);
	}

    public static void SaveUpgradeLevel(Upgrade u) {
        PlayerPrefs.SetInt(u.name, u.CurrentLevel);
    }

	public static LevelInfo LoadLevelInfo(int index) {
        string level = "Level" + index.ToString();
        bool locked = true;
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
            LevelInfo i = new LevelInfo(index, false, false, false, true);
            i.locked = i.index < 2 ? false : true;
            SaveLevelInfo(i);
            return i;
        }

        LevelInfo info = new LevelInfo (index, s1, s2, s3, locked);
		return info;
	}

    public static int LoadUpgradeLevel(string name) {
        if (PlayerPrefs.HasKey(name))
            return PlayerPrefs.GetInt(name);
        return 1;
    }
}