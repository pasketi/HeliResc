using UnityEngine;
using System.Collections;

[System.Serializable]
public class Upgrade {

    [HideInInspector]
    public string name;

    public Sprite upgradeSprite;

    //default level is 1
	private int currentLevel;
    public int maxLevel = 10;

    public int CurrentLevel {
        get {
            return currentLevel;
        }
        set {
            currentLevel = value >= maxLevel ? maxLevel : value;
        }
    }

    public int startLevel;
    public int priceMultiplier;

    public int UpgradePrice { get { return (priceMultiplier * (int)Mathf.Pow(2, currentLevel)); } }
    
    public void Init(string name) {
        this.name = name;        
        currentLevel = SaveLoad.LoadUpgradeLevel(name);
        if (currentLevel < 0) currentLevel = startLevel;

        //Create the key for the upgrade
        SaveLoad.SaveUpgrade(this);
    }
}
