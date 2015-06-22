using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {

	/*private GameObject reset, saved, cargo;
	private RectTransform tempRect, cargoImage;
	private Text tempText;
	private float scale = 0.1f;*/

	public bool refill = false;

	private Image fuel, fuelBorder;
	private Slider power;
	private bool 	lowFuel = false;
	private Text saved, cargo, action;
	private float 	flashLength = 0.25f,
					tempTime = 0f,
					cycle;
	private LevelManager manager;
	private Copter copter;
	private Color	red = new Color(1f, 0f, 0f),
					black = new Color(0f, 0f, 0f),
					white = new Color(1f, 1f, 1f);

	// Use this for initialization
	void Start () {
		manager = GameObject.Find("LevelManagerO").GetComponent<LevelManager>();
		copter = GameObject.Find ("Copter").GetComponent<Copter> ();

		saved = transform.FindChild ("SavedBackground").FindChild ("Saved").GetComponent<Text> ();
		cargo = transform.FindChild ("CargoBackground").FindChild ("Cargo").GetComponent<Text> ();
		//action = transform.FindChild ("ActionBackground").FindChild ("Action").GetComponent<Text> ();
		power = transform.FindChild ("PowerMeter").FindChild("Power").GetComponent<Slider> ();
		fuel = transform.FindChild ("Fuel").GetComponent<Image> ();
		fuelBorder = fuel.transform.FindChild ("FuelMeter").GetComponent<Image>();

        //if (manager.levelAction != 0) 
        //    transform.FindChild ("ActionBackground").FindChild ("IsKill").GetComponent<Button> ().onClick.AddListener(() => copter.UseAction ());
        //else
        //    transform.FindChild ("ActionBackground").GetComponent<Image>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		cycle = Mathf.Sin (Time.time*4);
		if (cycle < 0)
			cycle = -cycle;

        fuel.fillAmount = copter.fuelTank.CurrentFuelPercentage;
		power.value = copter.engine.CurrentPowerPercentage;
		saved.text = manager.getSavedCrates ().ToString () + "/" + manager.getCrateAmount ().ToString ();
		cargo.text = manager.cargoCrates.ToString () + "/" + manager.cargoSize.ToString ();
        //if (manager.levelAction != 0)
        //    action.text = manager.getActionsLeft().ToString () + "/" + manager.maxActionsPerLevel.ToString ();
        //else {
        //    action.text = "";

        //}

		if (manager.cargoCrates == manager.cargoSize)
			cargo.color = red;
		else
			cargo.color = black;

        if (copter.fuelTank.CurrentFuelPercentage >= 0.5f)
			fuel.color = new Color ((copter.fuelTank.maxCapacity - copter.fuelTank.CurrentFuel) / (copter.fuelTank.maxCapacity * 0.5f), 1f, 0f);
		else 
			fuel.color = new Color (1f, (copter.fuelTank.CurrentFuel / (copter.fuelTank.maxCapacity * 0.5f)), 0f);

        if (copter.fuelTank.CurrentFuelPercentage < 0.3f)
			lowFuel = true;
		else {
			lowFuel = false;
			fuelBorder.color = white;
		}

		if (!refill) {
			if (lowFuel) {
				tempTime += Time.deltaTime;
				if (tempTime >= flashLength) {
					tempTime = 0f;
					if (fuelBorder.color == white)
						fuelBorder.color = red;
					else if (fuelBorder.color == red)
						fuelBorder.color = white;
				}
			} else {
				fuelBorder.color = white;
			}
		} else {
			fuelBorder.color = new Color (cycle, 1f, cycle);
		}
	}
}
