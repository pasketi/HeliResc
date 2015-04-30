using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CopterEntryScript : MonoBehaviour {

    public Text copterName;
	public Text copterPrice;
    public Image copterImage;
	public Button buttonInfo;
    public Button buttonBuy;
    public Button buttonSelect;

    private string maxEngine;
    private string maxRope;
    private string maxFuel;

    private string engine;
    private string rope;
    private string fuel;

    private int index;

	private CopterSelection copterSelect;

    private GameManager gameManager;	
	
    public void SetCopterInfo(int index, Sprite s, CopterSelection copter) {

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

		copterSelect = copter;

        this.index = index;

        string[,] copters = gameManager.getCopters();
        string unlocked = copters[index, 3];
        bool copterUnlocked = int.Parse(unlocked) > 0;

		copterPrice.text = copters [index, 1];        

        copterName.text = copters[index, 0];

		copterImage.sprite = s;

        SetUpgrades();

		buttonInfo.onClick.AddListener (() => SetInfoPanel());
		buttonSelect.onClick.AddListener (() => PressSelect ());
		buttonBuy.onClick.AddListener (() => PressBuy ());

		buttonBuy.gameObject.SetActive(!copterUnlocked);
		buttonSelect.gameObject.SetActive(copterUnlocked);
    }

    public void SetUpgrades() {
        maxEngine = gameManager.GetUpgrade("Engine", index).maxLevel.ToString();
        maxFuel = gameManager.GetUpgrade("Fuel", index).maxLevel.ToString();
        maxRope = gameManager.GetUpgrade("Rope", index).maxLevel.ToString();

        engine = gameManager.GetUpgrade("Engine", index).CurrentLevel.ToString();
        fuel = gameManager.GetUpgrade("Fuel", index).CurrentLevel.ToString();
        rope = gameManager.GetUpgrade("Rope", index).CurrentLevel.ToString();

        Debug.Log("Engine: " + engine +  "/" + maxEngine + "   Fuel: " + fuel + "/" + maxFuel + "   Rope: " + rope + "/" + maxRope);
    }

	private void SetInfoPanel() {
		SetUpgrades ();
		string e = engine + "/" + maxEngine;
		string f = fuel + "/" + maxFuel;
		string r = rope + "/" + maxRope;
		copterSelect.SetCopterInfoPanel (e, f, r, index);
	}

	private void PressBuy() {
		gameManager.BuyCopter (index);
		UpdateUpgradeScreen ();
	}

	private void PressSelect() {
		gameManager.setCurrentCopter (index);
		UpdateUpgradeScreen ();
	}

	private void UpdateUpgradeScreen() {
		UpgradeButton[] buttons = GameObject.FindObjectsOfType<UpgradeButton> ();
		foreach (UpgradeButton ub in buttons)
			ub.UpdateTextFields ();
	}
}
