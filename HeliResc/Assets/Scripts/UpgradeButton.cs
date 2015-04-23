using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpgradeButton : MonoBehaviour {

    public string upgradeName;

    public Text upgradeLevel;
    public Text price;

    private GameManager gameManager;

	// Use this for initialization
	void Start () {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        UpdateTextFields();
    }

    public void PressedButton() {
        gameManager.BuyUpgrade(upgradeName);     
        UpdateTextFields();
    }

    private void UpdateTextFields() {
        Upgrade u = gameManager.GetUpgrade(upgradeName);

        upgradeLevel.text = u.CurrentLevel.ToString() + "/" + u.maxLevel.ToString();
        price.text = u.upgradePrice.ToString();
    }
}
