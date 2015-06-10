using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpgradeButton : MonoBehaviour {

    public Upgrade upgrade;
    public Upgrade Upgrade { set { upgrade = value; UpdateTextFields(); } }

    //Sprites to show as the progress of the upgrades level instead of text like 1/10
    public Sprite filledBallot;
    public Sprite emptyBallot;  

    //Unity UI Elements
    public Text txtUpgradeLevel;
    public Text txtPrice;
    public Image imgUpgradeSprite;
    public RectTransform rectLevelBallots;

    private GameManager gameManager;       

    public void PressedButton() {
        gameManager.BuyUpgrade(upgrade);
        UpdateTextFields();
    }

    public void UpdateTextFields() {
        
        txtUpgradeLevel.text = upgrade.CurrentLevel.ToString() + "/" + upgrade.maxLevel.ToString();
        DrawBallots(upgrade.CurrentLevel, upgrade.maxLevel);

        txtPrice.text = upgrade.UpgradePrice.ToString();
        imgUpgradeSprite.sprite = upgrade.upgradeSprite;
        
    }

    private void DrawBallots(int level, int max) {
        //instantiate as many empty ballots as the value of max and fill the amount of level variable

        //TODO
    }
}
