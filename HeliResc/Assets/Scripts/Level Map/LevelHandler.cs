using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelHandler : MonoBehaviour {

    public List<LevelSet> levelSets;
    public List<LevelSet> LevelSets { get { return instance.levelSets; } }
    public static Dictionary<string, List<Level>> Levels;

    private static LevelHandler instance;

	// Use this for initialization
	void Awake () {
        DontDestroyOnLoad(gameObject);
        instance = this;
        Levels = new Dictionary<string, List<Level>>();
	}
    
}

public class Level { 
    
}

[System.Serializable]
public class LevelSet {
    public string levelSetName;     //The identifier of the set. Crate, swimmer etc.
    public int levelAmount;         //how many levels is in the set
    public Sprite setImage;         //The image to show in the middle of the set
}
