using UnityEngine;
using System.Collections;

public class Upgrade {

    public string name;

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

    public int upgradePrice;

    public Upgrade(string name, int price) {
        this.name = name;
        upgradePrice = price;
        currentLevel = SaveLoad.LoadUpgradeLevel(name);
        
        //Create the key for the upgrade
        SaveLoad.SaveUpgradeLevel(this);
    }
}
