using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CopterSpecsMenu : MonoBehaviour {

	public Image copterImage;			//The UI image component
	public Text copterName;				//The text component to show copter name
	public Text copterDescription;		//The text component to show copter description
	public Transform ballotPanelFuel;	//The parent to assign ballots to show Fuel tank specs
	public Transform ballotPanelEngine;	//The parent to assign ballots to show Engine specs
	public Transform ballotPanelCargo;	//The parent to assign ballots to show Cargo specs
	public GameObject ballotPrefab;		//Prefab to show in copter specs

	void Start() {
		GridLayoutGroup[] grids = GetComponentsInChildren<GridLayoutGroup> ();

		float x = Screen.width * (28f / 1920f);
		float y = Screen.height * (89f / 1080f);

		foreach (GridLayoutGroup g in grids) {
			g.cellSize = new Vector2(x, y);
		}

	}

	public void UpdateSpecs(CopterInfo info) {
		DestroyBallots ();

		copterImage.sprite = info.copterSprite;

		if (info.unlocked == false && info.buyable == false) {
			copterImage.color = Color.black;
			copterName.text = "";
			copterDescription.text = "";

			foreach(Image i in gameObject.GetComponentsInChildren<Image>()) {
				i.enabled = false;
			}
			GetComponent<Image>().enabled = true;
		} 
		else {

			foreach(Image i in gameObject.GetComponentsInChildren<Image>()) {
				i.enabled = true;
			}

			copterImage.color = info.copterColor;
			copterName.text = info.copterName;
			copterDescription.text = info.description;

			int fuel = (int)(info.fuelAmount / 20);
			int cargo = info.cargoSpace;
			int engine = (int)(info.enginePower / 20);

			CreateBallots (ballotPanelFuel, fuel);
			CreateBallots (ballotPanelEngine, engine);
			CreateBallots (ballotPanelCargo, cargo);
		}
	}

	private void CreateBallots(Transform tr, int amount) {
		for (int i = 0; i < amount; i++) {
			GameObject g = Instantiate (ballotPrefab) as GameObject;
			g.transform.SetParent (tr);
		}
	}
	private void DestroyBallots() {
		foreach (Transform t in ballotPanelFuel) {
			Destroy(t.gameObject);
		}
		foreach (Transform t in ballotPanelEngine) {
			Destroy(t.gameObject);
		}
		foreach (Transform t in ballotPanelCargo) {
			Destroy(t.gameObject);
		}
	}
}
