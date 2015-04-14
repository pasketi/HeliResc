using UnityEngine;
using System.Collections;

public class Wallet {

    private GameManager manager;
    private int coins;
    public int Coins { get { return coins; } }

    public Wallet(int coinAmount) {
        coins = 0;
        AddMoney(coinAmount);
    }

    public void AddMoney(int amount) {
        coins += amount;
    }

    public void SaveWallet() {
        SaveLoad.SaveWallet(this);
    }

    /// <summary>
    /// Adds upgrade to players copter
    /// </summary>
    /// <returns>If the purchase was succesful</returns>
    public bool BuyUpgrade() {
        return false;
    }

    /// <summary>
    /// Unlocks a new copter if player has the money
    /// </summary>
    /// <returns>If the purchase was successful</returns>
    public bool BuyCopter() {
        return false;
    }
}
