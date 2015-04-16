using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MoneyUIUpdate : MonoBehaviour {

    private GameManager manager;
    private Text moneyText;

	// Use this for initialization
	void Start () {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        moneyText = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.I))
            manager.wallet.AddMoney(1);
        moneyText.text = "Money: " + manager.wallet.Coins.ToString();
	}
}
