using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CopterSelection : MonoBehaviour {

    private GameManager gameManager;							//Reference to the game manager

    private Dictionary<int, CopterEntryScript> copterEntries;	//The entries to show in the list of copters
    private Dictionary<int, CopterInfo> allCopters;				//Information about the copters

	//All these 3 panels are below the copter image in the copter info panel 
	public GameObject buyPanel;									//The panel that is available when the player hasn't bought the copter
	public GameObject lockedPanel;								//The panel that is on when player has not enough stars to buy the copter
	public GameObject unlockedPanel;							//The panel to show when the copter is available

	public Image lockedPanelStarImage;
	public Sprite rubySprite;
	public Sprite starSprite;

    public GridLayoutGroup group;								//Reference to layoutgroup component of the copterlist
    public GameObject copterEntry;								//Prefab of the copter entry to add to the list

	private int selectedCopter;									//Index of the selected copter
	private CopterSpecsMenu specs;

    void Start() {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		specs = GameObject.FindObjectOfType<CopterSpecsMenu> ();

		RectTransform t = transform as RectTransform;
		group.cellSize = new Vector2 (t.rect.width, t.rect.height * 0.25f);

		selectedCopter = gameManager.CurrentCopterIndex;

		copterEntries = new Dictionary<int, CopterEntryScript> ();
		allCopters = gameManager.GetCopterInfo();

		for (int i = 0; i < allCopters.Count; i++) {
			GameObject go = Instantiate(copterEntry) as GameObject;
			go.transform.SetParent(group.transform);
			CopterEntryScript script = go.GetComponent<CopterEntryScript>();
			script.SetInfo(i, this, allCopters[i]);

			copterEntries.Add(i, script);
		}

		copterEntries [selectedCopter].SelectCopter ();
		specs.UpdateSpecs (allCopters [selectedCopter]);
    } 

	public void CopterUnlocked() {
		buyPanel.SetActive (false);
		lockedPanel.SetActive (false);

		unlockedPanel.SetActive (true);
	}

	public void CopterLocked(string text, bool showStar) {
		buyPanel.SetActive (false);
		unlockedPanel.SetActive (false);

		lockedPanel.SetActive (true);

		lockedPanelStarImage.sprite = showStar ? starSprite : rubySprite;

		Text starText = lockedPanel.GetComponentInChildren<Text> ();
		starText.text = text;
	}

	public void CopterBuyable(string buyText) {
		unlockedPanel.SetActive (false);
		lockedPanel.SetActive (false);

		buyPanel.SetActive (true);

		Text moneyText = buyPanel.GetComponentInChildren<Text> ();
		moneyText.text = buyText;
	}

	public void UpdateSelected(int index) {
		selectedCopter = index;

		foreach (CopterEntryScript entry in copterEntries.Values) {
			entry.ShowBackground(false);
		}

		UpdateCopterScreen ();
	}

	public void UpdateCopterScreen() {
		CopterInfo info = allCopters [selectedCopter];

		specs.UpdateSpecs (info);
	}

	public void PressSelect() {
		GameManager.LoadLevel ("LevelMap");
	}
	public void PressBuy() {
		CopterInfo info = allCopters [selectedCopter];
		if (gameManager.wallet.Purchase(info.copterPrice) == true) {
			info.unlocked = true;
			info.Save();
            GetComponent<SoundObject>().PlayUsingHandler();
			copterEntries[selectedCopter].UpdateInfo(info);
			copterEntries[selectedCopter].SelectCopter();
		}
	}
}
