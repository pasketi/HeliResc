using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Wallet {

    private GameManager manager;
    private int coins;
    public int Coins { get { return coins; } }
        
    public static Dictionary<string, Upgrade> allUpgrades;

    public Wallet(int coinAmount) {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();

        GameObject[] copters = manager.copters;
        coins = 0;
        AddMoney(coinAmount);
        return;
        Debug.Log("Wallet constructor");

        allUpgrades = new Dictionary<string, Upgrade>();
        foreach (GameObject go in copters)
        {
            return;
            Dictionary<string, Upgradable> ups = go.GetComponent<Copter>().Upgrades;
            foreach (Upgradable up in ups.Values)
            {
                //Upgrade u = up.upgrade;
                //allUpgrades.Add(u.name, u);
                Debug.Log((up == null) + " up is null");
            }            
        }
    }

    public void AddMoney(int amount) {
        coins += amount;
    }

    private void Purchase(int amount) {
        if (coins - amount >= 0)
            coins -= amount;

    }

    public void SaveWallet() {
        SaveLoad.SaveWallet(this);
    }

    public int UpgradeLevel(string upgrade) {
        int copter = manager.CurrentCopterIndex;
        return allUpgrades["Copter" + copter.ToString() + upgrade].CurrentLevel;
    }

    public Upgrade GetUpgrade(string upgrade) {
        int copter = manager.CurrentCopterIndex;
        Upgrade u = allUpgrades["Copter" + copter.ToString() + upgrade];
        return u;
    }

    public Upgrade GetUpgrade(string upgrade, int index)
    {        
        Upgrade u = allUpgrades["Copter" + index.ToString() + upgrade];
        return u;
    }

    /// <summary>
    /// Adds upgrade to players copter
    /// </summary>
    /// <returns>If the purchase was succesful</returns>
    public bool BuyUpgrade(string upgrade) {
        Debug.Log("Upgrade: " + upgrade);
        int copter = manager.CurrentCopterIndex;
        Upgrade u = allUpgrades["Copter" + copter.ToString() + upgrade];
        if (Coins >= u.UpgradePrice && (u.CurrentLevel < u.maxLevel)) {
            allUpgrades[u.name].CurrentLevel++;
            Purchase(u.UpgradePrice);
            SaveLoad.SaveUpgrade(u);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Unlocks a new copter if player has the money
    /// </summary>
    /// <returns>If the purchase was successful</returns>
    public bool BuyCopter(int boughtCopter) {
		string[,] copters = manager.getCopters();
		if (Coins >= int.Parse(copters[boughtCopter, 1]) && copters[boughtCopter, 3] == "0") {
			Purchase (int.Parse (copters[boughtCopter, 1]));
			SaveLoad.SaveWallet(this);
			return true;
		} else
        	return false;
    }
}
