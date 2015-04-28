using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CopterEntryScript : MonoBehaviour {

    public Text copterName;
    public Image copterImage;
    public Button buttonBuy;
    public Button buttonSelect;

    private string maxEngine;
    private string maxRope;
    private string maxFuel;

    private string engine;
    private string rope;
    private string fuel;

    private int index;

    private GameManager gameManager;	
	
    public void SetCopterInfo(int index) {

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        this.index = index;

        string[,] copters = gameManager.getCopters();
        string unlocked = copters[index, 3];
        bool copterUnlocked = int.Parse(unlocked) > 0;

        buttonBuy.gameObject.SetActive(copterUnlocked);
        buttonSelect.gameObject.SetActive(copterUnlocked);

        copterName.text = copters[index, 0];

        SetUpgrades();
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
}
