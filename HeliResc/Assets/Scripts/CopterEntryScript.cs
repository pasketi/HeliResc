using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CopterEntryScript : MonoBehaviour {    

    public int index;						//The index of the copter in the gamemanagers copter list
	public Image copterImage;				//The image of the copter to show in the selection list. Get the sprite from gamemanager

	private CopterSelection copterSelect;	//Reference to the copterselect script

	private Image backgroundImage;			//The background sprite. Turn off if the copter is not selected
    private GameManager gameManager;		//Reference to the game manager
	private CopterInfo copter;				//The copter the button will show
	

	void Start() {
		Button button = GetComponent<Button> ();
		button.onClick.AddListener (() => SelectCopter ());
	}

	public void SetInfo(int index, CopterSelection select, CopterInfo copter) {

		backgroundImage = GetComponent<Image> ();

		this.index = index;
		this.copter = copter;
		copterSelect = select;
		gameManager = GameObject.FindObjectOfType<GameManager> ();

		backgroundImage.enabled = false;
		copterImage.sprite = copter.copterSprite;
		copterImage.color = copter.copterColor;

		if (copter.unlocked == false && copter.buyable == false) {
			copterImage.color = Color.black;
		}
	}
	public void UpdateInfo(CopterInfo info) {
		this.copter = info;
	}

	public void SelectCopter() {
		copterSelect.UpdateSelected (index);

		ShowBackground (true);
		if (copter.unlocked == true) {

			gameManager.CurrentCopterIndex = index;
			PlayerPrefs.SetInt(SaveStrings.sSelectedCopter, index);
			copterSelect.CopterUnlocked();
			
		} else if (copter.buyable == true) {

			string buyText = copter.copterPrice.ToString();
			copterSelect.CopterBuyable(buyText);
			
		} else {
			string starText = "";
			bool showStar = false;
			if(copter.requiredStars > 0){
				starText = PlayerPrefs.GetInt(SaveStrings.sPlayerStars) + "/" + copter.requiredStars.ToString();
				showStar = true;
			}
			else if(copter.requiredRubies > 0){
				starText = PlayerPrefs.GetInt(SaveStrings.sPlayerRubies) + "/" + copter.requiredRubies.ToString();
			}
			copterSelect.CopterLocked(starText, showStar);
			
		}
	}
	public void ShowBackground(bool show) {
		backgroundImage.enabled = show;
	}
}
