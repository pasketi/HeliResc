using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelHandler : MonoBehaviour {

    public LevelSet[] LevelSets;
    public static Dictionary<string, List<Level>> Levels;       //all the levels in the game. the key string is the name of the set of levels
    public static Level CurrentLevel { get { return instance.currentLevel; } }
    public Level currentLevel;
    public string currentSet;

    private static LevelHandler instance;

	// Use this for initialization
	void Awake () {
        DontDestroyOnLoad(gameObject);
        instance = this;

        //Initialize the levels list
        Levels = new Dictionary<string, List<Level>>();
        //LevelSetHandler[] sets = GameObject.FindObjectsOfType<LevelSetHandler>();
        for (int j = 0; j < LevelSets.Length; j++ )
        {
            List<Level> l = new List<Level>();
            for (int i = 0; i < LevelSets[j].levelAmount; i++)
            {
                l.Add(new Level(LevelSets[j].levelSetName, i));
            }
            Levels.Add(LevelSets[j].levelSetName, l);
        }
	}
    public static LevelSet GetLevelSet(string name) {
        foreach (LevelSet s in instance.LevelSets) {
            if (s.levelSetName.Equals(name))
                return s;
        }
        return null;
    }
    public static void UpdateCurrentLevel(Level l) {
        instance.currentSet = l.setName;
        instance.currentLevel = Levels[l.setName][l.id];
    }
    public static void CompleteLevel(Level l) {
        Level.Save(l);
        int id = l.id;
        List<Level> levels = Levels[l.setName];
        LevelSet nextSet = null;
        //First unlock the next set.
        for (int i = 0; i < instance.LevelSets.Length; i++) { 
            if(i + 1 < instance.LevelSets.Length && instance.LevelSets[i].levelSetName.Equals(l.setName)) {
                nextSet = instance.LevelSets[i + 1];
            }
        }
        if (nextSet != null && nextSet.unlocked == false) {
            nextSet.unlocked = true;
            nextSet.Save();
        }
        
        //Second unlock the next level in the current set if there is more levels
        if (l.id + 1 < levels.Count) {
            Level nextLevel = levels[l.id + 1];

            //If the level is not yet unlocked, unlock the level and save the value
            if (nextLevel.unlocked == false) {
                nextLevel.unlocked = true;
                Level.Save(nextLevel);
            }
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
