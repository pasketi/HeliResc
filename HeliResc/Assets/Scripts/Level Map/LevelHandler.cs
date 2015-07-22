using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelHandler : MonoBehaviour {

    public static Dictionary<string, List<Level>> Levels;       //all the levels in the game. the key string is the name of the set of levels

    private static LevelHandler instance;

	// Use this for initialization
	void Awake () {
        DontDestroyOnLoad(gameObject);
        instance = this;

        //Initialize the levels list
        Levels = new Dictionary<string, List<Level>>();
        LevelSetHandler[] sets = GameObject.FindObjectsOfType<LevelSetHandler>();
        foreach (LevelSetHandler handler in sets) {
            List<Level> l = new List<Level>();
            for (int i = 0; i < handler.set.levelAmount; i++) {
                l.Add(new Level(handler.set.levelSetName, i));
            }
            Levels.Add(handler.set.levelSetName, l);
        }
	}
}

public class Level {

    public int id;
    public string setName;    
    public string name;
    public bool star1;
    public bool star2;
    public bool star3;
    public bool rubyFound;
    public bool unlocked;

    public Level(string setName, int id) {
        this.setName = setName;
        this.id = id;
        name = setName + id;
    }

    public static Level Load(string setName, int id) {
        Level l = new Level(setName, id);

        l.LoadInfo();
        if (setName == "Tutorial" && id == 0)
            l.unlocked = true;

        return l;
    }
    public static void Save(Level level) {
        PlayerPrefsExt.SetBool(level.name + "Star1", level.star1);
        PlayerPrefsExt.SetBool(level.name + "Star2", level.star2);
        PlayerPrefsExt.SetBool(level.name + "Star3", level.star3);
        PlayerPrefsExt.SetBool(level.name + "Ruby", level.rubyFound);
        PlayerPrefsExt.SetBool(level.name + "Unlocked", level.unlocked);
    }

    public void LoadInfo() {
        if (PlayerPrefs.HasKey(name + "Star1")) { star1 = PlayerPrefsExt.GetBool(name + "Star1"); }
        else { PlayerPrefsExt.SetBool(name + "Star1", false); }

        if (PlayerPrefs.HasKey(name + "Star2")) { star2 = PlayerPrefsExt.GetBool(name + "Star2"); }
        else { PlayerPrefsExt.SetBool(name + "Star2", false); }

        if (PlayerPrefs.HasKey(name + "Star3")) { star3 = PlayerPrefsExt.GetBool(name + "Star3"); }
        else { PlayerPrefsExt.SetBool(name + "Star3", false); }

        if (PlayerPrefs.HasKey(name + "Ruby")) { rubyFound = PlayerPrefsExt.GetBool(name + "Ruby"); }
        else { PlayerPrefsExt.SetBool(name + "Ruby", false); }

        if (PlayerPrefs.HasKey(name + "Unlocked")) { unlocked = PlayerPrefsExt.GetBool(name + "Unlocked"); }
        else { PlayerPrefsExt.SetBool(name + "Unlocked", false); }
    }
}
