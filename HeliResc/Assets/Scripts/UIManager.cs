using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {

	/*private GameObject reset, saved, cargo;
	private RectTransform tempRect, cargoImage;
	private Text tempText;
	private float scale = 0.1f;*/

	private Image fuel, fuelBorder;
	private Slider power;
	private bool lowFuel = false;
	private Text saved, cargo;
	private float 	flashLength = 0.25f,
					tempTime = 0f;
	private LevelManager manager;
	private CopterManagerTouch copter;
	private Color	red = new Color(1f, 0f, 0f),
					black = new Color(0f, 0f, 0f),
					white = new Color(1f, 1f, 1f);

	// Use this for initialization
	void Start () {
		manager = GameObject.Find("LevelManagerO").GetComponent<LevelManager>();
		copter = GameObject.Find ("Copter").GetComponent<CopterManagerTouch> ();

		saved = transform.FindChild ("SavedBackground").FindChild ("Saved").GetComponent<Text> ();
		cargo = transform.FindChild ("CargoBackground").FindChild ("Cargo").GetComponent<Text> ();
		power = transform.FindChild ("PowerMeter").FindChild("Power").GetComponent<Slider> ();
		fuel = transform.FindChild ("Fuel").GetComponent<Image> ();
		fuelBorder = fuel.transform.FindChild ("FuelMeter").GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
		fuel.fillAmount = copter.getFuel () / copter.maxFuel;
		power.value = copter.getPower () / (copter.maxPower-copter.minPower);
		saved.text = manager.getSavedCrates ().ToString () + "/" + manager.getCrateAmount ().ToString ();
		cargo.text = manager.cargoCrates.ToString () + "/" + manager.cargoSize.ToString ();

		if (manager.cargoCrates == manager.cargoSize)
			cargo.color = red;
		else
			cargo.color = black;

		if (copter.getFuel () / copter.maxFuel >= 0.5f)
			fuel.color = new Color ((copter.maxFuel - copter.getFuel ()) / (copter.maxFuel - (copter.maxFuel / 2)), 1f, 0f);
		else 
			fuel.color = new Color (1f, (copter.getFuel () / (copter.maxFuel - (copter.maxFuel / 2))), 0f);

		if (copter.getFuel () / copter.maxFuel < 0.3f)
			lowFuel = true;
		else {
			lowFuel = false;
			fuelBorder.color = white;
		}


		if (lowFuel) {
			tempTime += Time.deltaTime;
			if (tempTime >= flashLength) {
				tempTime = 0f;
				if (fuelBorder.color == white)
					fuelBorder.color = red;
				else if (fuelBorder.color == red)
					fuelBorder.color = white;
			}
		}
	}
}
