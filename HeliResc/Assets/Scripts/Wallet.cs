using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Wallet {

    private GameManager manager;
    private int coins;
    public int Coins { get { return coins; } }

    private string[] upNames =  { "Fuel", "Engine", "Rope" };
    private int[] upPrices =    { 1, 2, 3 };
    public static Dictionary<string, Upgrade> allUpgrades;

    public Wallet(int coinAmount) {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        coins = 0;
        AddMoney(coinAmount);

        int copterAmount = manager.CopterAmount;

        allUpgrades = new Dictionary<string, Upgrade>();
        for (int i = 0; i < copterAmount; i++)
        {
            for (int j = 0; j < upNames.Length; j++)
            {
                Upgrade u = new Upgrade("Copter" + i.ToString() + upNames[j], upPrices[j]);
                allUpgrades.Add(u.name, u);
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
        int copter = manager.getCurrentCopter();
        return allUpgrades["Copter" + copter.ToString() + upgrade].CurrentLevel;
    }

    public Upgrade GetUpgrade(string upgrade) {
        int copter = manager.getCurrentCopter();
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
        int copter = manager.getCurrentCopter();
        Upgrade u = allUpgrades["Copter" + copter.ToString() + upgrade];
        if (Coins >= u.upgradePrice && (u.CurrentLevel < u.maxLevel)) {
            allUpgrades[u.name].CurrentLevel++;
            Purchase(u.upgradePrice);
            SaveLoad.SaveUpgradeLevel(u);
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
