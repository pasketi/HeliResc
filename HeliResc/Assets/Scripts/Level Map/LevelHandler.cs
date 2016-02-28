using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelHandler : MonoBehaviour {

    public LevelSet[] LevelSets;
	public Level currentLevel;
	public string currentSet;

	public static Dictionary<string, List<Level>> Levels;       //all the levels in the game. the key string is the name of the set of levels
	public static Level CurrentLevel { get { return instance.currentLevel; } 
		set { 
			instance.currentLevel = value;
            instance.currentSet = value.setName;
			PlayerPrefs.SetInt(SaveStrings.sCurrentLevelIndex, value.id);
			PlayerPrefs.SetString(SaveStrings.sCurrentLevelSet, value.setName);
		} 
	}
    public static LevelSet[] levelSets { get { return instance.LevelSets; } }    
	public static int LevelCount;

    private static LevelHandler instance;

	// Use this for initialization
	void Awake () {
        DontDestroyOnLoad(gameObject);
        instance = this;
		LevelCount = 0;

        //Initialize the levels list
        Levels = new Dictionary<string, List<Level>>();
        //LevelSetHandler[] sets = GameObject.FindObjectsOfType<LevelSetHandler>();
        for (int j = 0; j < LevelSets.Length; j++ )
        {
            LevelSets[j].setIndex = j;
            List<Level> l = new List<Level>();
            for (int i = 0; i < LevelSets[j].levelAmount; i++)
            {
				LevelCount++;
                l.Add(Level.Load(LevelSets[j].levelSetName, i));
            }
            Levels.Add(LevelSets[j].levelSetName, l);
        }
        foreach (LevelSet set in LevelSets) {            
            set.Load();
        }
		currentSet = PlayerPrefs.GetString (SaveStrings.sCurrentLevelSet, "Tutorial0");
		currentLevel = Levels[currentSet][PlayerPrefs.GetInt(SaveStrings.sCurrentLevelIndex, 0)];
	}

    public static LevelSet GetLevelSet(string name = "") {
        if (name.Equals(""))
            name = instance.currentSet;
        foreach (LevelSet s in instance.LevelSets) {
            if (s.levelSetName.Equals(name)) {
                s.Load();
                return s;
            }
        }
        return null;
    }
    
    public static void CompleteLevel(Level l) {
        Level.Save(l);
        List<Level> levels = Levels[l.setName];
        LevelSet nextSet = null;
		CurrentLevel = l;

        //First unlock the next set.
        for (int i = 0; i < instance.LevelSets.Length; i++) { 
            if(i + 1 < instance.LevelSets.Length && instance.LevelSets[i].levelSetName.Equals(l.setName)) {
                nextSet = instance.LevelSets[i + 1];
            }
        }
        if (nextSet != null && nextSet.unlocked == false) {
            //Debug.Log("Next Set");
            nextSet.unlocked = true;
            nextSet.Save();
            Level nextLevel = Levels[nextSet.levelSetName][0];
            if (nextLevel.unlocked == false) {
                nextLevel.unlocked = true;
                Level.Save(nextLevel);
            }
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
	public float levelTimeChallenge;
	public float bestTime;

    public Level(string setName, int id) {
        this.setName = setName;
        this.id = id;
        name = setName + id;
    }

    public static Level Load(string setName, int id) {
        Level l = new Level(setName, id);

		l.levelTimeChallenge = LevelHandler.GetLevelSet (setName).levelTimeChallenges [id];

        l.LoadInfo();
        if (setName == "Tutorial0" && id == 0)
            l.unlocked = true;

        return l;
    }
    public static void Save(Level level) {
        PlayerPrefsExt.SetBool(level.name + "Star1", level.star1);
        PlayerPrefsExt.SetBool(level.name + "Star2", level.star2);
        PlayerPrefsExt.SetBool(level.name + "Star3", level.star3);
        PlayerPrefsExt.SetBool(level.name + "Ruby", level.rubyFound);
        PlayerPrefsExt.SetBool(level.name + "Unlocked", level.unlocked);
		PlayerPrefs.SetFloat(level.name + "BestTime", level.bestTime);
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

		if (PlayerPrefs.HasKey(name + "BestTime")) { bestTime = PlayerPrefs.GetFloat(name + "BestTime"); }
		else { PlayerPrefs.SetFloat(name + "BestTime", 999f); }
    }

	public bool checkLevelCompletion (Level level) {
		return (level.star1 && level.star2 && level.star3 && level.rubyFound) ? true : false ;
	}

    public override string ToString()
    {
        string str = "Unlocked: " + unlocked;
        str += " : Ruby: " + rubyFound;
        str += " : Set name: " + setName;
        str += " : ID: " + id;

        return str;
    }
}
