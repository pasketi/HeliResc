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

    private GameManager gameManager;	
	
    public void SetCopterInfo(int index) {

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        string[,] copters = gameManager.getCopters();
        string unlocked = copters[index, 3];
        bool copterUnlocked = int.Parse(unlocked) > 0;

        buttonBuy.gameObject.SetActive(copterUnlocked);
        buttonSelect.gameObject.SetActive(copterUnlocked);

        copterName.text = copters[index, 0];

        maxEngine = gameManager.GetUpgrade("Engine").maxLevel.ToString();
        maxFuel = gameManager.GetUpgrade("Fuel").maxLevel.ToString();
        maxRope = gameManager.GetUpgrade("Rope").maxLevel.ToString();

        engine = gameManager.GetUpgrade("Engine").CurrentLevel.ToString();
        fuel = gameManager.GetUpgrade("Fuel").CurrentLevel.ToString();
        rope = gameManager.GetUpgrade("Rope").CurrentLevel.ToString();
    }
}
