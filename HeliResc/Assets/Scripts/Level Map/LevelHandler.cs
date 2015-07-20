using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelHandler : MonoBehaviour {

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
