using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {

	/*private GameObject reset, saved, cargo;
	private RectTransform tempRect, cargoImage;
	private Text tempText;
	private float scale = 0.1f;*/

	private RectTransform health, fuel;
	private bool lowFuel = false, lowHealth = false;
	private Text saved, cargo;
	private Slider power;
	private float 	healthMin, 
					healthMax,
					fuelMin, 
					fuelMax,
					flashLength = 0.25f,
					tempTime = 0f;
	private LevelManager manager;
	private CopterManagerTouch copter;
	private Color	red = new Color(1f, 0f, 0f),
					black = new Color(0f, 0f, 0f),
					white = new Color(1f, 1f, 1f),
					lightGrey = new Color(0.75f, 0.75f, 0.75f);

	// Use this for initialization
	void Start () {
		manager = GameObject.Find("LevelManagerO").GetComponent<LevelManager>();
		copter = GameObject.Find ("Copter").GetComponent<CopterManagerTouch> ();

		saved = transform.FindChild ("Saved").GetComponent<Text> ();
		cargo = transform.FindChild ("Cargo").GetComponent<Text> ();
		power = transform.FindChild ("Power").GetComponent<Slider> ();

		health = transform.FindChild ("Health").GetComponent<RectTransform> ();
		healthMin = health.anchorMin.y;
		healthMax = health.anchorMax.y;

		fuel = transform.FindChild ("Fuel").GetComponent<RectTransform> ();
		fuelMin = fuel.anchorMin.x;
		fuelMax = fuel.anchorMax.x;
	}
	
	// Update is called once per frame
	void Update () {
		health.anchorMax = new Vector2 (health.anchorMax.x, ((copter.getHealth () / copter.maxHealth) * (healthMax - healthMin)) + healthMin);
		fuel.anchorMax = new Vector2 (((copter.getFuel() / copter.maxFuel) * (fuelMax-fuelMin)) + fuelMin, fuel.anchorMax.y);
		saved.text = manager.getSavedCrates ().ToString () + "/" + manager.getCrateAmount ().ToString ();
		cargo.text = manager.cargoCrates.ToString () + "/" + manager.cargoSize.ToString ();
		power.value = copter.getPower () / (copter.maxPower - copter.minPower);

		if (manager.cargoCrates == manager.cargoSize)
			cargo.color = red;
		else
			cargo.color = black;

		if (copter.getFuel () / copter.maxFuel < 0.3f)
			lowFuel = true;
		else {
			lowFuel = false;
			fuel.GetComponent<Image> ().color = white;
		}

		if (copter.getHealth () / copter.maxHealth < 0.3f)
			lowHealth = true;
		else {
			lowHealth = false;
			health.GetComponent<Image> ().color = white;
		}

		if (lowFuel || lowHealth) {
			tempTime += Time.deltaTime;
			if (tempTime >= flashLength) {
				tempTime = 0f;

				if (lowFuel) {
					if (fuel.GetComponent<Image> ().color == white)
						fuel.GetComponent<Image> ().color = lightGrey;
					else if (fuel.GetComponent<Image> ().color == lightGrey)
						fuel.GetComponent<Image> ().color = white;
				}

				if (lowHealth) {
					if (health.GetComponent<Image> ().color == white)
						health.GetComponent<Image> ().color = lightGrey;
					else if (health.GetComponent<Image> ().color == lightGrey)
						health.GetComponent<Image> ().color = white;
				}
			}
		}
	}
}
