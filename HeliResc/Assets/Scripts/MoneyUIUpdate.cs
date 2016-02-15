using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MoneyUIUpdate : MonoBehaviour {

    private GameManager manager;
	private int currentMoney, oldMoney;
    private Text moneyText;
	private bool levelEnd = false;

	// Use this for initialization
	void Start () {
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        moneyText = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		currentMoney = manager.wallet.Coins;
        if (Input.GetKeyDown(KeyCode.I))
            manager.wallet.AddMoney(1);
		if (!levelEnd || oldMoney == currentMoney) moneyText.text = manager.wallet.Coins.ToString();
		else moneyText.text = oldMoney.ToString();
		
		if (oldMoney == currentMoney) setLevelEnd(false);
	}

		public void setOldMoney (int old) {
				oldMoney = old;
		}

		public int getOldMoney () {
				return oldMoney;
		}

		public void setLevelEnd (bool x) {
				levelEnd = x;
		}

		public void addCoin () {
				oldMoney++;
		}
}
