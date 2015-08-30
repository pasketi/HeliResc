using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Wallet {

    private GameManager manager;
    private int coins;
    public int Coins { get { return coins; } }            

    public Wallet(int coinAmount) {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();

        coins = 0;
        AddMoney(coinAmount);
    }

    public void AddMoney(int amount) {
        coins += amount;
        SaveWallet();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="price"></param>
    /// <returns>true if the player has the money</returns>
    public bool Purchase(int price) {
        if (coins - price >= 0) {
            coins -= price;
            return true;
        }
        return false;
    }

    public void SaveWallet() {
        SaveLoad.SaveWallet(this);
    }

}
